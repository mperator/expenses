using Expenses.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        //Task<User> GetCurrentUserAsync();
    }
}
