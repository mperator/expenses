using Expenses.Api.Entities;
using Expenses.Api.Models;
using Expenses.Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenOptions _options;

        public AuthController(UserManager<User> userManager, IOptions<JwtTokenOptions> options)
        {
            _userManager = userManager;
            _options = options.Value;
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

        [HttpPost("login")]
        public async Task<ActionResult<TokenModel>> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = null;
            if(user == null && !string.IsNullOrEmpty(model.Username))
                user = await _userManager.FindByNameAsync(model.Username);
            else if (user == null && !string.IsNullOrEmpty(model.Email))
                user = await _userManager.FindByEmailAsync(model.Email);
            else
            {
                return BadRequest("Could not login.");
            }

            if(!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest("Could not login.");
            }

            // Create token
            return GenerateToken(user);
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<string> TestAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            return $"Secret for {user.FirstName} {user.LastName}";
        }

        public void Logout()
        {

        }

        public void RefreshToken()
        {

        }

        private TokenModel GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(30));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenModel
            {
                AccessToken = accessToken,
                ExpiryDate = expires
            };
        }
    }
}
