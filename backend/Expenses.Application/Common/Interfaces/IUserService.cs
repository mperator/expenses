using Expenses.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> FindByIdAsync(string id);
        Task<AppUser> GetCurrentUserAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync(string name);
    }
}
