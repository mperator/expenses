using Expenses.Api.Common;
using Expenses.Application.Features.Expenses.Commands.CreateExpense;
using Expenses.Application.Features.Expenses.Commands.DeleteExpense;
using Expenses.Application.Features.Expenses.Commands.UpdateExpense;
using Expenses.Application.Features.Expenses.Queries.GetExpenseById;
using Expenses.Application.Features.Expenses.Queries.GetExpenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetExpensesQueryExpense>>> GetExpensesAsync(int eventId)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetExpenseByIdQueryExpense>> GetExpenseById(int eventId, int expenseId)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateExpenseAsync(int eventId, int expenseId, [FromBody] UpdateExpenseRequestExpense model)
        {
            var expense = await Mediator.Send(new UpdateExpenseCommand { EventId = eventId, ExpenseId = expenseId, Model = model });
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteExpenseAsync(int eventId, int expenseId)
        {
            await Mediator.Send(new DeleteExpenseCommand { EventId = eventId, ExpenseId = expenseId });
            return NoContent();
        }
    }
}
