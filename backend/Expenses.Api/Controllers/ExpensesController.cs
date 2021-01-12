using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
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
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        #region Constructor

        public ExpensesController(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all expenses belonging to an event
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <returns>A list of expenses</returns>
        /// <response code="400">No expense found for the given expense and event id</response>
        /// <response code="200">On success</response>
        [HttpGet()]
        public async Task<ActionResult> GetExpensesAsync(int eventId)
        {
            var dbEvent = await _dbContext.EventData
                .Include(ev => ev.Expenses)
                .SingleOrDefaultAsync(ev => ev.Id == eventId);

            if (dbEvent == null) return BadRequest();

            var eventExpensesList = dbEvent.Expenses;
            // there are no expenses for this event yet
            if (eventExpensesList == null) return Ok(new List<ExpenseReadModel>());
            List<ExpenseReadModel> expenses = new();
            foreach (var expense in eventExpensesList)
            {
                expenses.Add(_mapper.Map<ExpenseReadModel>(expense));
            }
            
            return Ok(expenses);
        }
        /// <summary>
        /// Get a single expense using its ID
        /// </summary>
        /// <param name="eventId">ID of the event to which the expense belongs</param>
        /// <param name="expenseId">ID of the expense</param>
        /// <returns>A single expense object</returns>
        /// <response code="404">No expense found for the given expense and event id</response>
        /// <response code="200">On success</response>
        [HttpGet("{expenseId}", Name = nameof(GetExpenseById))]
        public async Task<ActionResult<ExpenseReadModel>> GetExpenseById(int eventId, int expenseId)
        {
            var dbExpense = await _dbContext.ExpenseData
                .Include(ex => ex.ExpensesUsers)
                .FirstOrDefaultAsync(ex => ex.EventId == eventId && ex.Id == expenseId);
            if (dbExpense == null) return NotFound();

            return Ok(_mapper.Map<ExpenseReadModel>(dbExpense));
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
        public async Task<ActionResult<ExpenseReadModel>> CreateExpenseAsync(int eventId, [FromBody] ExpenseWriteModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == eventId);
            if (dbEvent == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var expenseToAdd = _mapper.Map<Expense>(model);
            expenseToAdd.EventId = eventId;
            expenseToAdd.Event = dbEvent;
            expenseToAdd.Issuer = user;
            expenseToAdd.IssuerId = user.Id;
            expenseToAdd.Currency = "EUR";

            _dbContext.ExpenseData.Add(expenseToAdd);
            dbEvent.Expenses.Add(expenseToAdd);

            _dbContext.ExpenseUsers.AddRange(model.Participants.Select(e => new ExpenseUser
            {
                Amount = e.Amount,
                Expense = expenseToAdd,
                UserId = e.Id
            }));

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                new InvalidOperationException(e.Message);
            }

            return CreatedAtRoute(nameof(GetExpenseById), new { eventId = eventId, expenseId = expenseToAdd.Id }, _mapper.Map<ExpenseReadModel>(expenseToAdd));
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
            var update = _mapper.Map<Expense>(model);
            //TODO: make sure that only same user as creator or a user with appropriate role can change event

            var dbExpense = await _dbContext.ExpenseData.FirstOrDefaultAsync(ex => ex.EventId == eventId && ex.Id == expenseId);
            if (dbExpense == null) return NotFound();

            dbExpense.Title = update.Title;
            dbExpense.Description = update.Description;
            dbExpense.Date = update.Date;
            dbExpense.Amount = update.Amount;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                new InvalidOperationException(e.Message);
            }

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
            var dbExpense = await _dbContext.ExpenseData.FirstOrDefaultAsync(ex => ex.EventId == eventId && ex.Id == expenseId);
            if (dbExpense == null) return NotFound();

            _dbContext.Remove(dbExpense);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                new InvalidOperationException(e.Message);
            }

            return NoContent();
        }

        #endregion
    }
}
