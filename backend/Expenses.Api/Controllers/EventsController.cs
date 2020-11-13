using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public EventsController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventReadModel>>> GetEventsAsync()
        {
            //FIXME: check whether requesting user is authorized
            // if not return 401 Unauthorized
            return Ok(_mapper.Map<List<EventReadModel>>(await _dbContext.EventData.ToListAsync()));
        }

        [HttpGet("{id}", Name = nameof(GetEventByIdAsync))]
        public async Task<ActionResult<EventReadModel>> GetEventByIdAsync(int? id)
        {
            if (id == null) return BadRequest();

            Event foundEvent = await _dbContext.EventData.SingleOrDefaultAsync(ev => ev.Id == id);
            if (foundEvent == null) return NotFound();
            
            return Ok(_mapper.Map<EventReadModel>(foundEvent));
        }

        [HttpPost]
        public async Task<ActionResult<EventReadModel>> CreateEventAsync([FromBody] EventWriteModel eventModel)
        {
            //FIXME: check for authorization

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newEvent = _mapper.Map<Event>(eventModel);

            //TODO: what to put inside BadRequest?
            if (newEvent == null) return BadRequest();
            //Event newEvent;
            Event savedEvent = new Event
            {
                Title = newEvent.Title,
                Description = newEvent.Description,
                Creator = newEvent.Creator,
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
            return Ok(savedEvent);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEventAsync(int? id, [FromBody] EventUpdateModel model)
        {
            if (id == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var update = _mapper.Map<Event>(model);

            if (update == null) return BadRequest();

            var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == id);

            if (dbEvent == null) return BadRequest();

            dbEvent.Title = update.Title;
            dbEvent.StartDate = update.StartDate;
            dbEvent.EndDate = update.EndDate;
            dbEvent.Description = update.Description;
            dbEvent.Creator = update.Creator;
            dbEvent.Currency = update.Currency;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEventByIdAsync(int? id)
        {
            if (id == null) return BadRequest();

            var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == id);

            if (dbEvent == null) return BadRequest();

            _dbContext.Remove(dbEvent);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
