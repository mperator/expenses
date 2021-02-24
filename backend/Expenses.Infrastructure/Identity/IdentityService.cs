using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using System;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        public Task<Result> ConfirmEmail(string email, string token)
        {
            throw new NotImplementedException();
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

        public Task<Result> RegisterAsync(string firstName, string lastName, string username, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
