using Expenses.Application.Common.Interfaces;
using Expenses.Domain.EntitiesOld;
using Expenses.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        //TODO: own identity context
        public async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string name)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(name))
                query = query.Where(u =>
                    u.FirstName.Contains(name) ||
                    u.LastName.Contains(name) ||
                    u.UserName.Contains(name));

            return (await query.ToListAsync()).Select(s => (User)s);
        }
    }
}
