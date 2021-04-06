using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using Expenses.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    public class TestController : ApiControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            //var @event = new Event(new User("49e877a7-7ecb-4255-af00-fad57dcaec74"), "My first domain event", "This is a description of the domain event.", DateTime.Now, DateTime.Now.AddDays(10), "EUR");
            //_context.Events.Add(@event);
            //await _context.SaveChangesAsync(new System.Threading.CancellationToken());

            //var events = await _context.Events.AsNoTracking()
            //    .Include(a => a.Expenses)
            //    .ToListAsync();

            var @event = await _context.Events
                .Include(a => a.Expenses)
                .FirstOrDefaultAsync(a => a.Id == 1);

            var creditor = @event.Creator;
            var debitor = new User("9dc88d66-2c4a-41a0-8b67-f68029fab52f");
            var debitor2 = new User("e88a8e0a-070d-4b37-b1df-ec0130da6ea3");

            @event.AddParticipant(debitor2);

            var expense = new Expense(creditor, "My fist expense", "My Expense descritpion", DateTime.Now.AddHours(1), @event.Currency);
            expense.Split(
                new Credit(creditor, 100),
                new List<Debit>
                {
                    new Debit(creditor, 20),
                    new Debit(debitor, 50),
                    new Debit(debitor2, 30)
                });

            @event.AddExpense(expense);

            try
            {
                await _context.SaveChangesAsync(new System.Threading.CancellationToken());
            }
            catch (Exception ex)
            {

            }



            return Ok();
        }
    }
}
