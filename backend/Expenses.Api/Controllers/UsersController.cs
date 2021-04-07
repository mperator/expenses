using Expenses.Api.Common;
using Expenses.Application.Features.Users.Queries.SearchUsersByName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchUsersByNameQueryUser>>> GetAttendeesAsync([FromQuery] string query)
        {
            return Ok(await Mediator.Send(new SearchUsersByNameQuery { SearchText = query }));
        }
    }
}
