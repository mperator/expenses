﻿using Expenses.Api.Entities;
using Expenses.Api.Models;
using Expenses.Api.Options;
using Expenses.Application.Common.Interfaces;
using Expenses.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // https://www.codewithmukesh.com/blog/refresh-tokens-in-aspnet-core/

        private readonly UserManager<User> _userManager;
        private readonly IFeatureManager _featureManager;
        private readonly JwtTokenOptions _options;
        private readonly IIdentityService _identityService;

        //public AuthController(
        //    UserManager<User> userManager,
        //    IFeatureManager featureManager,
        //    IOptions<JwtTokenOptions> options)
        //{
        //    _userManager = userManager;
        //    _featureManager = featureManager;
        //    _options = options.Value;
        //}

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Endpoint to test authorization. For demo purpose only.
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        //[HttpGet("test")]
        //public async Task<string> TestAsync()
        //{
        //    var user = await _userManager.GetUserAsync(User);

        //    return $"Secret for {user.FirstName} {user.LastName} {Guid.NewGuid()}";
        //}

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="201">On success.</response>
        /// <response code="400">Registration could not be proceeded.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationModel model)
        {
            var confirmationLink = $"{Request.Scheme}://{Request.Host.Value}{Url.RouteUrl(nameof(ConfirmEmail))}";
            var result = await _identityService.RegisterAsync(model.FirstName, model.LastName, model.Username, model.Email, model.Password,
                confirmationLink);

            if (result.Succeeded) return NoContent();
            else return BadRequest(result.Errors);
        }

        // TODO: Better create post.
        // user navigates on email confirmation sites with params. Site requests for email confirm with posts and redirect depending on data. Eventually requests new confirmation link. Or navigate to login.
        // TODO: Link which is send by email must manually encode token value because it canc contain + sings which will removed. Use: https://www.urlencoder.org
        [HttpGet("confirmEmail", Name = nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await _identityService.ConfirmEmailAsync(email, token);
            if (result.Succeeded)
                return NoContent();
            else
                return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login a user. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns access and refresh token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<Expenses.Application.Common.Models.TokenModel>> LoginAsync([FromBody] LoginModel model)
        {
            var (result, token, refreshToken) = await _identityService.LoginAsync(model.Username, model.Email, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            SetRefreshTokenInCookie(refreshToken);

            return token;
        }

        /// <summary>
        /// Log out user. Revokes all refresh tokens.
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _identityService.LogoutAsync(User);

            if (result) Response.Cookies.Delete("X-RefreshToken");

            return NoContent();
        }
        //TODO: do we need to get rid of the models?
        /// <summary>
        /// Refresh token by token value in body.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenModel model)
        {
            var result = await _identityService.HandleRefreshTokenAsync(model.Token);

            if (!result.Result.Succeeded) return Unauthorized(result.Result.Errors);
            else return Ok(result.TokenModel.AccessToken);
        }

        /// <summary>
        /// Refresh token silent by token in cookie.
        /// </summary>
        /// <returns></returns>
        [HttpPost("refreshTokenSilent")]
        public async Task<IActionResult> RefreshTokenByCookieAsync()
        {
            var result = await _identityService.HandleRefreshTokenAsync(Request.Cookies["X-RefreshToken"]);

            if (!result.Result.Succeeded) return Unauthorized(result.Result.Errors);
            else
                SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result.TokenModel.AccessToken);
        }






        /// <summary>
        /// Set RefreshToken in Cookie.
        /// </summary>
        /// <param name="refreshToken"></param>
        private void SetRefreshTokenInCookie(Expenses.Application.Common.Models.RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };
            Response.Cookies.Append("X-RefreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
