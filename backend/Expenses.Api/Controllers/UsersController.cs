using Expenses.Api.Common;
using Expenses.Application.Features.Users.Queries.SearchUserById;
using Expenses.Application.Features.Users.Queries.SearchUsersByName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    public class UsersFilterQuery
    {
        public string Name { get; set; }
    }

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<SearchUsersByNameQueryUser>> GetUsersByName([FromQuery] UsersFilterQuery query)
        {
            return await Mediator.Send(new SearchUsersByNameQuery { SearchText = query.Name });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SearchUserByIdQueryUser>> GetUserById(string id)
        {
            return Ok(await Mediator.Send(new SearchUserByIdQuery { Id = id }));
        }
    }
}
