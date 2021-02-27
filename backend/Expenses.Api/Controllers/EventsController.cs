using Expenses.Application.Events.Commands.CreateEvent;
using Expenses.Application.Events.Queries.GetEvents;
using Expenses.Application.Events.Queries.GetEventById;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    //TODO: implement clear error messages and return them to the user
    //FIXME: just for dev purpose
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiControllerBase
    {
        //private readonly AppDbContext _dbContext;
        //private readonly IMapper _mapper;
        //private readonly UserManager<User> _userManager;
        //public EventsController(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        //{
        //    _dbContext = dbContext;
        //    _mapper = mapper;
        //    _userManager = userManager;
        //}
        /// <summary>
        /// Gets a list of events
        /// </summary>
        /// <returns>A list of events</returns>
        /// <response code="200">On success</response>
        [HttpGet]
        public async Task<ActionResult<List<Application.Events.Queries.GetEvents.EventReadModel>>> GetEventsAsync()
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
        public async Task<ActionResult<Application.Events.Queries.GetEventById.EventReadModel>> GetEventByIdAsync(int id)
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
        /// <param name="model">Event data to update</param>
        /// <response code="400">Model isn't valid or mapping failed</response>
        /// <response code="404">No event to update found for the given ID </response>
        /// <response code="204">On success</response>
        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdateEventAsync(int id, [FromBody] EventUpdateModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var update = _mapper.Map<Event>(model);

        //    if (update == null) return BadRequest();

        //    var dbEvent = await _dbContext.EventData
        //        .Include(ev => ev.Creator)
        //        .Include(e => e.Attendees)
        //        .Include(ev => ev.Expenses)
        //        .AsSplitQuery()
        //        .FirstOrDefaultAsync(ev => ev.Id == id);

        //    if (dbEvent == null) return NotFound();

        //    dbEvent.Title = update.Title;
        //    dbEvent.StartDate = update.StartDate;
        //    dbEvent.EndDate = update.EndDate;
        //    dbEvent.Description = update.Description;
        //    //dbEvent.Creator = update.Creator;
        //    //dbEvent.Currency = update.Currency;

        //    foreach (var a in model.Attendees)
        //    {
        //        var attendee = await _userManager.FindByIdAsync(a.Id);
        //        dbEvent.Attendees.Add(attendee);
        //    }

        //    await _dbContext.SaveChangesAsync();

        //    return NoContent();
        //}
        /// <summary>
        /// Delete an event using its ID
        /// </summary>
        /// <param name="id">ID of event which shall be deleted</param>
        /// <response code="400">No ID given</response>
        /// <response code="404">No event to delete found for the given ID</response>
        /// <response code="204">On success</response>
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteEventByIdAsync(int? id)
        //{
        //    if (id == null) return BadRequest();

        //    var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == id);

        //    if (dbEvent == null) return NotFound();

        //    _dbContext.Remove(dbEvent);
        //    await _dbContext.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
