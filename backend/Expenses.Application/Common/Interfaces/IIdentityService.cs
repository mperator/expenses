using Expenses.Application.Common.Models;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        //Task<string> GetUserNameAsync(string userId);

        //Task<bool> IsInRoleAsync(string userId, string role);

        //Task<bool> AuthorizeAsync(string userId, string policyName);

        //Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        //Task<Result> DeleteUserAsync(string userId);

        Task<Result> ConfirmEmail(string email, string token);

 
        Task<(Result Result, TokenModel TokenModel)> HandleRefreshTokenAsync(string refreshToken);

        Task<(Result Result, TokenModel TokenModel)> LoginAsync(string username, string email, string password);

        Task<bool> LogoutAsync();

        Task<Result> RegisterAsync(string firstName, string lastName, string username, string email, string password);
    }
}
