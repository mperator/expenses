using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    public class TestController : ApiControllerBase
    {
        private readonly IAppDbContext _context;

        public TestController(IAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var @event = new Event(new User("c4cdfef6-6526-478a-99ad-19990c0e7667"), "My first domain event", "This is a description of the domain event.", DateTime.Now, DateTime.Now.AddDays(10), "EUR");

            _context.Events.Add(@event);
            await _context.SaveChangesAsync(new System.Threading.CancellationToken());


            var events = await _context.Events.AsNoTracking()
                .Include(a => a.Participants)
                .ToListAsync();

            return Ok();
        }
    }
}
