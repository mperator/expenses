using Expenses.Application.Features.Events.Commands.CreateEvent;
using Expenses.Application.Features.Events.Queries.GetEvents;
using Expenses.Application.Features.Events.Queries.GetEventById;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Expenses.Application.Features.Events.Commands.UpdateEvent;
using Expenses.Application.Features.Events.Commands.DeleteEvent;

namespace Expenses.Api.Controllers
{
    //TODO: implement clear error messages and return them to the user
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiControllerBase
    {
        /// <summary>
        /// Gets a list of events
        /// </summary>
        /// <returns>A list of events</returns>
        /// <response code="200">On success</response>
        [HttpGet]
        public async Task<ActionResult<List<Application.Features.Events.Queries.GetEvents.GetEventsQueryEvent>>> GetEventsAsync()
        {
            return await Mediator.Send(new GetEventsQuery());
        }
        /// <summary>
        /// Get a single event by its ID
        /// </summary>
        /// <param name="id">Unique ID to identify event</param>
        /// <returns>A single event object</returns>
        /// <response code="200">On success</response>
        /// <response code="400">No ID given</response>
        /// <response code="404">No resource found for the given ID</response>
        [HttpGet("{id}", Name = nameof(GetEventByIdAsync))]
        public async Task<ActionResult<Application.Features.Events.Queries.GetEventById.EventReadModel>> GetEventByIdAsync(int id)
        {
            return await Mediator.Send(new GetEventByIdQuery { Id = id });
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="eventCommand">Event which shall be created</param>
        /// <returns>Created event object</returns>
        /// <response code="400">Mapping failed or model isn't valid</response>
        /// <response code="201">Returns created event object</response>
        [HttpPost]
        public async Task<ActionResult<int>> CreateEventAsync([FromBody] CreateEventCommand eventCommand)
        {
            return await Mediator.Send(eventCommand);
        }
        /// <summary>
        /// Update an event by replacing it
        /// </summary>
        /// <param name="id">ID of event which shall be updated</param>
        /// <param name="updateEventCommand">Event data to update</param>
        /// <response code="400">Model isn't valid or mapping failed</response>
        /// <response code="404">No event to update found for the given ID </response>
        /// <response code="204">On success</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEventAsync(int id, [FromBody] UpdateEventCommand updateEventCommand)
        {
            updateEventCommand.Id = id;

            await Mediator.Send(updateEventCommand);
            return NoContent();
        }
        /// <summary>
        /// Delete an event using its ID
        /// </summary>
        /// <param name="id">ID of event which shall be deleted</param>
        /// <response code="400">No ID given</response>
        /// <response code="404">No event to delete found for the given ID</response>
        /// <response code="204">On success</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEventByIdAsync(int id)
        {
            await Mediator.Send(new DeleteEventCommand { Id = id });
            return NoContent();
        }
    }
}
