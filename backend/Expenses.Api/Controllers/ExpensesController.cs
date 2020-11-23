﻿using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    //TODO: documentation
    [Authorize]
    [ApiController]
    [Route("api/events/{eventId}/[controller]")]
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ExpensesController(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [HttpGet()]
        public async Task<ActionResult> GetExpensesAsync(int eventId)
        {
            var dbEvent = await _dbContext.EventData.Include(ev => ev.Expenses)
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
            //abfrage gib mir alle expenses für das event mit der id
            return Ok(expenses);
        }
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
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                new InvalidOperationException(e.Message);
            }

            // TODO: createdatroute, test and workout
            return Ok(_mapper.Map<ExpenseReadModel>(expenseToAdd));
        }
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
    }
}
