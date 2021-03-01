using Expenses.Application.Features.Attendees.Queries.GetAttendees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAttendeesResponseAttendee>>> GetAttendeesAsync([FromQuery] GetAttendeesRequestAttendeeFilter filter)
        {
            return Ok(await Mediator.Send(new GetAttendeesQuery { Filter = filter }));
        }
    }
}
