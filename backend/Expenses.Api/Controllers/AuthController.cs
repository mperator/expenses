using Expenses.Api.Common;
using Expenses.Application.Common.Models;
using Expenses.Application.Features.Auth.Commands.ConfirmEmail;
using Expenses.Application.Features.Auth.Commands.Login;
using Expenses.Application.Features.Auth.Commands.Logout;
using Expenses.Application.Features.Auth.Commands.RefreshCurrentToken;
using Expenses.Application.Features.Auth.Commands.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="201">On success.</response>
        /// <response code="400">Registration could not be proceeded.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequestModel model)
        {
            var confirmationLink = $"{Request.Scheme}://{Request.Host.Value}{Url.RouteUrl(nameof(ConfirmEmail))}";
            var result = await Mediator.Send(new RegisterUserCommand { ConfirmationLink = confirmationLink, Model = model });

            return result.Succeeded ? NoContent() : BadRequest(result.Errors);
        }


        // TODO: Better create post.
        // user navigates on email confirmation sites with params. Site requests for email confirm with posts and redirect depending on data. Eventually requests new confirmation link. Or navigate to login.
        // TODO: Link which is send by email must manually encode token value because it canc contain + sings which will removed. Use: https://www.urlencoder.org
        [HttpGet("confirmEmail", Name = nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await Mediator.Send(new ConfirmEmailCommand { Email = email, Token = token });

            return result.Succeeded ? NoContent() : BadRequest(result.Errors);
        }

        /// <summary>
        /// Login a user. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns access and refresh token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<TokenModel>> LoginAsync([FromBody] LoginCommand model)
        {
            var (result, token, refreshToken) = await Mediator.Send(model);
            
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
            var result = await Mediator.Send(new LogoutCommand());

            if (result) Response.Cookies.Delete("X-RefreshToken");
            return NoContent();
        }
        
        /// <summary>
        /// Refresh token by token value in body.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<ActionResult<TokenModel>> RefreshTokenAsync([FromBody] RefreshTokenCommand request)
        {
            var (result, token, refreshToken) = await Mediator.Send(request);
            if (!result.Succeeded) return Unauthorized(result.Errors);
            else
            {
                SetRefreshTokenInCookie(refreshToken);
                return Ok(token);
            }
        }

        /// <summary>
        /// Refresh token silent by token in cookie.
        /// </summary>
        /// <returns></returns>
        [HttpPost("refreshTokenSilent")]
        public async Task<ActionResult<TokenModel>> RefreshTokenByCookieAsync()
        {
            var (result, token, refreshToken) = await Mediator.Send(new RefreshTokenCommand { Token = Request.Cookies["X-RefreshToken"] });
            if (!result.Succeeded) return Unauthorized(result.Errors);
            else
            {
                SetRefreshTokenInCookie(refreshToken);
                return Ok(token);
            }
        }

        /// <summary>
        /// Set RefreshToken in Cookie.
        /// </summary>
        /// <param name="refreshToken"></param>
        private void SetRefreshTokenInCookie(RefreshToken refreshToken)
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
