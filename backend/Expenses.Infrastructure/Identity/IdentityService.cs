using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeatureManager _featureManager;
        private readonly IEmailService _emailService;

        public IdentityService(UserManager<ApplicationUser> userManager, IFeatureManager featureManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _featureManager = featureManager;
            _emailService = emailService;
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

        public Task<(Result Result, TokenModel TokenModel)> HandleRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<(Result Result, TokenModel TokenModel)> LoginAsync(string username, string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
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

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            //var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            //return await _userManager.IsInRoleAsync(user, role);
            throw new NotImplementedException();
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            //var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            //return user.UserName;
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
