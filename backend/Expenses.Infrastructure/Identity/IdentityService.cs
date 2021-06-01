using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using Expenses.Infrastructure.Exceptions;
using Expenses.Infrastructure.Options;
using Expenses.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFeatureManager _featureManager;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly JwtTokenOptions _options;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IFeatureManager featureManager,
            IEmailService emailService,
            IOptions<JwtTokenOptions> options,
            AppDbContext context)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _featureManager = featureManager;
            _emailService = emailService;
            _context = context;
            _options = options.Value;
        }

        public async Task<Result> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure(new List<string> { "Invalid link." });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return Result.Failure(result.Errors.Select(e => e.Description));

            return Result.Success();
        }

        public async Task<(Result Result, TokenModel TokenModel, RefreshToken refreshToken)> LoginAsync(string username, string email, string password)
        {
            // only username and password is mandatory
            if (string.IsNullOrEmpty(username)) throw new IdentityException("Username must not be empty.");
            if (string.IsNullOrEmpty(password)) throw new IdentityException("Password must not be empty.");

            ApplicationUser user = null;
            if (user == null && !string.IsNullOrEmpty(username))
                user = await _userManager.FindByNameAsync(username);
            else if (user == null && !string.IsNullOrEmpty(email))
                user = await _userManager.FindByEmailAsync(email);
            else
                throw new IdentityException("Invalid username or password.");

            // Check if eemail confirmation is enabled.
            if (await _featureManager.IsEnabledAsync("EmailConfirmation"))
            {
                // check if user email is confirmend
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    throw new IdentityException("Please confirm email.");
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new IdentityException("Invalid username or password.");

            var refreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (Result.Failure(result.Errors.Select(e => e.Description)), null, null);

            // Create token
            var token = GenerateToken(user);
            token.RefreshToken = refreshToken.Token;
            token.RefreshTokenExpires = refreshToken.Expires;

            return (Result.Success(), token, refreshToken);
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // Delete cookie invalidate all refresh tokens.
            var activeTokens = user.RefreshTokens.Where(x => x.IsActive);
            foreach (var token in activeTokens)
            {
                token.Revoked = DateTime.UtcNow;
            }
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<Result> RegisterAsync(string firstName, string lastName, string username,
            string email, string password, string registerLink)
        {
            if (firstName == null || lastName == null || username == null || email == null || password == null)
            {
                return Result.Failure(Enumerable.Empty<string>());
            }

            if (await _userManager.FindByNameAsync(username) != null)
                return Result.Failure(new List<string> { "Username is invalid or already taken." });
            if (await _userManager.FindByEmailAsync(email) != null)
                return Result.Failure(new List<string> { "Email is invalid or already taken." });

            // add user to context send user an email he needs to verify
            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                //TODO: add DoB
                DateOfBirth = null
            }, password);

            if (!result.Succeeded)
            {
                return Result.Failure(result.Errors.Select(e => e.Description));
            }

            // Send email if feature is enabled.
            if (await _featureManager.IsEnabledAsync("EmailConfirmation"))
            {
                var user = await _userManager.FindByEmailAsync(email);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = $"{registerLink}?email={user.Email}&token={token}";


                await _emailService.SendAsync(user.Email, "Confirm your email", confirmationLink);
            }

            return Result.Success();
        }

        /// <summary>
        /// Generate a refresh token for user.
        /// </summary>
        /// <returns></returns>
        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddSeconds(_options.RefreshTokenExpiryTimeInSeconds),
                    Created = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Generate an access token for user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private TokenModel GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddSeconds(_options.AccessTokenExpiryTimeInSeconds);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenModel
            {
                TokenType = "Bearer",
                AccessToken = accessToken,
                Expires = expires
            };
        }

        /// <summary>
        /// Check if request is allowd to generate new refresh and access tokens.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<(Result Result, TokenModel TokenModel, RefreshToken RefreshToken)> HandleRefreshTokenAsync(string refreshToken)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                // return error and null
                return (Result.Failure(new List<string> { "Token did not match any users." }), null, null);
            }

            // Check if token is active
            var token = user.RefreshTokens.Single(x => x.Token == refreshToken);
            if (!token.IsActive)
            {
                return (Result.Failure(new List<string> { "Token expired." }), null, null);
            }

            token.Revoked = DateTime.UtcNow;

            // generate new refresh and access token

            var newRefreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);

            //SetRefreshTokenInCookie(newRefreshToken);

            // Create token
            var newAccessToken = GenerateToken(user);
            //FIXME: was ist mit all den anderen props?
            newAccessToken.RefreshToken = newRefreshToken.Token;
            newAccessToken.RefreshTokenExpires = newRefreshToken.Expires;
            await _context.SaveChangesAsync();

            return (Result.Success(), newAccessToken, newRefreshToken);

            throw new Exception();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            //var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            //return await _userManager.IsInRoleAsync(user, role);
            throw new NotImplementedException();
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            //var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            //var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            //var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            //return result.Succeeded;
            throw new NotImplementedException();
        }


    }
}
