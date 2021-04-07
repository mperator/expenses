using Expenses.Application.Common.Interfaces;
using Expenses.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor
            //, UserManager<ApplicationUser> userManager
            )

        {
            _httpContextAccessor = httpContextAccessor;
            //_userManager = userManager;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        //public async Task<User> GetCurrentUserAsync()
        //{
        //    return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        //}
    }
}
