using Expenses.Api.Common;
using Expenses.Application.Features.Users.Queries.SearchUsersById;
using Expenses.Application.Features.Users.Queries.SearchUsersByName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    public class UsersFilterQuery
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchUsersByNameQueryUser>>> GetAttendeesAsync([FromQuery] UsersFilterQuery query)
        {
            // TODO: how to handle this correctly?
            if (query?.Name != null) return Ok(await Mediator.Send(new SearchUsersByNameQuery { SearchText = query.Name }));
            else if(query?.Id != null) return Ok(await Mediator.Send(new SearchUsersByIdQuery { Id = query.Id }));
            else return BadRequest();
        }
    }
}
