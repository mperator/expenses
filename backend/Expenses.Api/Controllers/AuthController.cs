using Expenses.Api.Data;
using Expenses.Api.Entities;
using Expenses.Api.Models;
using Expenses.Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly JwtTokenOptions _options;

        public AuthController(UserManager<User> userManager, AppDbContext context, IOptions<JwtTokenOptions> options)
        {
            _userManager = userManager;
            _context = context;
            _options = options.Value;
        }

        /// <summary>
        /// Endpoint to test authorization. For demo purpose only.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("test")]
        public async Task<string> TestAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            return $"Secret for {user.FirstName} {user.LastName}";
        }

        /// <summary>
        /// Registers a new user to Expenses.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _userManager.FindByNameAsync(model.Username) != null) return BadRequest("Username is invalid or already taken.");
            if (await _userManager.FindByEmailAsync(model.Email) != null) return BadRequest("Email is invalid or already taken.");

            // add user to context send user an email he needs to verify
            var result = await _userManager.CreateAsync(new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            }, model.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        /// <summary>
        /// Login a user. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns access and refresh token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<TokenModel>> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = null;
            if(user == null && !string.IsNullOrEmpty(model.Username))
                user = await _userManager.FindByNameAsync(model.Username);
            else if (user == null && !string.IsNullOrEmpty(model.Email))
                user = await _userManager.FindByEmailAsync(model.Email);
            else
            {
                return BadRequest("Could not login.");
            }

            if(!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest("Could not login.");
            }

            var refreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.First());
            };

            SetRefreshTokenInCookie(refreshToken);

            // Create token
            var token = GenerateToken(user);
            token.RefreshToken = refreshToken.Token;
            token.RefreshTokenExpires = refreshToken.Expires;

            return token;
        }

        /// <summary>
        /// Log out user. Revokes all refresh tokens.
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            // delete cookie invalidate all refresh tokens
            var user = await _userManager.GetUserAsync(User);
            var activeTokens = user.RefreshTokens.Where(x => x.IsActive);
            foreach(var token in activeTokens)
            {
                token.Revoked = DateTime.UtcNow;
            }
            await _userManager.UpdateAsync(user);

            Response.Cookies.Delete("X-RefreshToken");

            return NoContent();
        }

        /// <summary>
        /// Refresh token by token value in body.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenModel model)
        {
            return await HandleRefreshTokenAsync(model.Token);
        }

        /// <summary>
        /// Refresh token silent by token in cookie.
        /// </summary>
        /// <returns></returns>
        [HttpPost("refreshTokenSilent")]
        public async Task<IActionResult> RefreshTokenByCookieAsync()
        {
            return await HandleRefreshTokenAsync(Request.Cookies["X-RefreshToken"]);
        }

        /// <summary>
        /// Check if request is allowd to generate new refresh and access tokens.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<IActionResult> HandleRefreshTokenAsync(string refreshToken)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if(user == null)
            {
                return Unauthorized("Token did not match any users.");
            }

            // check if token is active
            var token = user.RefreshTokens.Single(x => x.Token == refreshToken);
            if(!token.IsActive)
            {
                return Unauthorized("Token expired.");
            }

            token.Revoked = DateTime.UtcNow;

            // generate new refresh and access token

            var newRefreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            
            SetRefreshTokenInCookie(newRefreshToken);

            // Create token
            var newAccessToken = GenerateToken(user);
            newAccessToken.RefreshToken = newRefreshToken.Token;
            newAccessToken.RefreshTokenExpires = newRefreshToken.Expires;
            await _context.SaveChangesAsync();

            return Ok(newAccessToken);
        }

        /// <summary>
        /// Generate an access token for user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private TokenModel GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(15);

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
        /// Generate a refresh token for user.
        /// </summary>
        /// <returns></returns>
        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(14),
                    Created = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Set RefreshToken in Cookie.
        /// </summary>
        /// <param name="refreshToken"></param>
        private void SetRefreshTokenInCookie(RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };
            Response.Cookies.Append("X-RefreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
