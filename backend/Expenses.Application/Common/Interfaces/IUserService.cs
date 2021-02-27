using Expenses.Domain.Entities;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByIdAsync(string id);

        Task<User> GetCurrentUserAsync();
    }
}
