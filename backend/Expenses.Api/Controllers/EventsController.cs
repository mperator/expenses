using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Api.Controllers
{
    //TODO: implement clear error messages and return them to the user
    //FIXME: just for dev purpose
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public EventsController(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }
        /// <summary>
        /// Gets a list of events
        /// </summary>
        /// <returns>A list of events</returns>
        /// <response code="200">On success</response>
        [HttpGet]
        public async Task<ActionResult<List<EventReadModel>>> GetEventsAsync()
        {
            return Ok(_mapper.Map<List<EventReadModel>>(await _dbContext.EventData.ToListAsync()));
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
        public async Task<ActionResult<EventReadModel>> GetEventByIdAsync(int? id)
        {
            if (id == null) return BadRequest();

            Event foundEvent = await _dbContext.EventData.SingleOrDefaultAsync(ev => ev.Id == id);
            if (foundEvent == null) return NotFound();
            
            return Ok(_mapper.Map<EventReadModel>(foundEvent));
        }
        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="eventModel">Event which shall be created</param>
        /// <returns>Created event object</returns>
        /// <response code="400">Mapping failed or model isn't valid</response>
        /// <response code="201">Returns created event object</response>
        [HttpPost]
        public async Task<ActionResult<EventReadModel>> CreateEventAsync([FromBody] EventWriteModel eventModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newEvent = _mapper.Map<Event>(eventModel);

            if (newEvent == null) return BadRequest();

            
            var user = await _userManager.GetUserAsync(User);

            Event savedEvent = new Event
            {
                Title = newEvent.Title,
                Description = newEvent.Description,
                Creator = user,
                Currency = "Euro",
                StartDate = newEvent.StartDate,
                EndDate = newEvent.EndDate
            };

            _dbContext.EventData.Add(savedEvent);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                new InvalidOperationException(e.Message);
            }
            return CreatedAtRoute(nameof(GetEventByIdAsync), new { id = savedEvent.Id }, savedEvent);
        }
        /// <summary>
        /// Update an event by replacing it
        /// </summary>
        /// <param name="id">ID of event which shall be updated</param>
        /// <param name="model">Event data to update</param>
        /// <response code="400">Model isn't valid or mapping failed</response>
        /// <response code="404">No event to update found for the given ID </response>
        /// <response code="204">On success</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEventAsync(int id, [FromBody] EventUpdateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var update = _mapper.Map<Event>(model);

            if (update == null) return BadRequest();

            var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == id);

            if (dbEvent == null) return NotFound();

            dbEvent.Title = update.Title;
            dbEvent.StartDate = update.StartDate;
            dbEvent.EndDate = update.EndDate;
            dbEvent.Description = update.Description;
            dbEvent.Creator = update.Creator;
            dbEvent.Currency = update.Currency;
            //dbEvent.Attendees = update.Attendees;

            await _dbContext.SaveChangesAsync();

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
        public async Task<ActionResult> DeleteEventByIdAsync(int? id)
        {
            if (id == null) return BadRequest();

            var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == id);

            if (dbEvent == null) return NotFound();

            _dbContext.Remove(dbEvent);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
