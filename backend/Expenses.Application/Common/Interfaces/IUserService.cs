using Expenses.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByIdAsync(string id);

        Task<User> GetCurrentUserAsync();
        Task<IEnumerable<User>> GetUsersAsync(string name);
    }
}
