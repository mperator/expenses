using Expenses.Application.Features.Events.Commands.CreateEvent;
using Expenses.Application.Features.Events.Queries.GetEventById;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Expenses.Application.Features.Events.Commands.UpdateEvent;
using Expenses.Application.Features.Events.Commands.DeleteEvent;
using Expenses.Api.Common;
using Expenses.Application.Features.Events.Queries.GetEventsByFilter;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

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
        /// <param name="parameter">Filter parameter.</param>
        /// <returns>A list of events</returns>
        /// <response code="200">On success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetEventsByFilterQueryEvent>>> GetEventsByFilterAsync([FromQuery] GetEventsByFilterQuery parameter)
        {
            var events = await Mediator.Send(parameter);

            var metadata = new
            {
                events.TotalCount,
                events.PageSize,
                events.CurrentPage,
                events.TotalPages,
                events.HasNext,
                events.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return events;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetEventByIdQueryEvent>> GetEventByIdAsync(int id)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteEventByIdAsync(int id)
        {
            await Mediator.Send(new DeleteEventCommand { Id = id });
            return NoContent();
        }
    }
}
