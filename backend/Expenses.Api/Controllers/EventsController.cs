using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Microsoft.AspNetCore.Http;
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

            Event foundEvent;
            //try
            //{
                foundEvent = await _dbContext.EventData.SingleOrDefaultAsync(ev => ev.Id == id);
            //}
            //catch (Exception e)
            //{
            //    new InvalidOperationException(e.Message);
            //}
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
                Currency = newEvent.Currency,
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


    }
}
