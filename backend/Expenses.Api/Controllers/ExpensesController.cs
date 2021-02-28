using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Expenses.Application.Features.Expenses.Commands.CreateExpense;
using Expenses.Application.Features.Expenses.Queries.GetExpenseById;
using Expenses.Application.Features.Expenses.Queries.GetExpenses;
using Expenses.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/events/{eventId}/[controller]")]
    public class ExpensesController : ApiControllerBase
    {
        /// <summary>
        /// Get a list of all expenses belonging to an event.
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <returns>A list of expenses</returns>
        /// <response code="400">No expense found for the given expense and event id</response>
        /// <response code="200">On success</response>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<GetExpensesExpenseModel>>> GetExpensesAsync(int eventId)
        {
            return Ok(await Mediator.Send(new GetExpensesQuery { EventId = eventId }));
        }

        /// <summary>
        /// Get a single expense using its id.
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <param name="expenseId">ID of the expense</param>
        /// <returns>A single expense object</returns>
        /// <response code="404">No expense found for the given expense and event id</response>
        /// <response code="200">On success</response>
        [HttpGet("{expenseId}", Name = nameof(GetExpenseById))]
        public async Task<ActionResult<ExpenseReadModel>> GetExpenseById(int eventId, int expenseId)
        {
            return Ok(await Mediator.Send(new GetExpenseByIdQuery { EventId = eventId, ExpenseId = expenseId }));
        }

        /// <summary>
        /// Create an expense
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <param name="model">Expense which shall be created</param>
        /// <returns>Created expense object</returns>
        /// <response code="400">Model isn't valid</response>
        /// <response code="404">No event found for the given id</response>
        /// <response code="201">Returns created expense object</response>
        [HttpPost()]
        public async Task<ActionResult<CreateExpenseResponseExpense>> CreateExpenseAsync(int eventId, [FromBody] CreateExpenseRequestExpense model)
        {
            var expense = await Mediator.Send(new CreateExpenseCommand { EventId = eventId, Model = model });
            return CreatedAtRoute(nameof(GetExpenseById), new { eventId, expenseId = expense.Id }, expense);
        }

        /// <summary>
        /// Update an expense
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <param name="expenseId">ID of the expense</param>
        /// <param name="model">Expense data to update</param>
        /// <response code="404">No expense found for the given expense and event id</response>
        /// <response code="204">Update has been succuessful</response>
        [HttpPut("{expenseId}")]
        public async Task<ActionResult<ExpenseReadModel>> UpdateExpenseAsync(int eventId, int expenseId, [FromBody] ExpenseUpdateModel model)
        {
            //var update = _mapper.Map<Expense>(model);
            ////TODO: make sure that only same user as creator or a user with appropriate role can change event

            //var dbExpense = await _dbContext.ExpenseData.FirstOrDefaultAsync(ex => ex.EventId == eventId && ex.Id == expenseId);
            //if (dbExpense == null) return NotFound();

            //dbExpense.Title = update.Title;
            //dbExpense.Description = update.Description;
            //dbExpense.Date = update.Date;
            //dbExpense.Amount = update.Amount;

            //try
            //{
            //    await _dbContext.SaveChangesAsync();
            //}
            //catch (Exception e)
            //{
            //    new InvalidOperationException(e.Message);
            //}

            return NoContent();
        }
        /// <summary>
        /// Delete an expense of an event using an expense id
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <param name="expenseId">ID of the expense</param>
        /// <response code="204">Expense successfully deleted</response>
        /// <response code="404">No expense found for the given expense and event id</response>
        [HttpDelete("{expenseId}")]
        public async Task<ActionResult> DeleteExpenseAsync(int eventId, int expenseId)
        {
            //var dbExpense = await _dbContext.ExpenseData.FirstOrDefaultAsync(ex => ex.EventId == eventId && ex.Id == expenseId);
            //if (dbExpense == null) return NotFound();

            //_dbContext.Remove(dbExpense);
            //try
            //{
            //    await _dbContext.SaveChangesAsync();
            //}
            //catch (Exception e)
            //{
            //    new InvalidOperationException(e.Message);
            //}

            return NoContent();
        }
    }
}
