using Expenses.Api.Entities;
using Expenses.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager)
        {
            _userManager = userManager;
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

        public void Login()
        {

        }

        public void Logout()
        {

        }

        public void RefreshToken()
        {

        }
    }
}
