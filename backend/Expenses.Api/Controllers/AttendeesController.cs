using Expenses.Application.Attendees.Queries.GetAttendees;
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
        public async Task<ActionResult<IEnumerable<GetAttendeesAttendeeReadModel>>> GetAttendeesAsync([FromQuery] AttendeeFilter filter)
        {
            return Ok(await Mediator.Send(new GetAttendeesQuery { Filter = filter }));
        }
    }
}
