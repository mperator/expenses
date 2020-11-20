using AutoMapper;
using Expenses.Api.Data;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    [ApiController]
    [Route("api/events/{id}/[controller]")]
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public ExpensesController(AppDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        //FIXME: check whether requesting user is authorized
        [HttpGet()]
        public async Task<ActionResult> GetExpensesAsync(int id)
        {

            //abfrage gib mir alle expenses für das event mit der id
            return null;
        }

        [HttpPost()]
        public async Task<ActionResult<ExpenseReadModel>> CreateExpenseAsync(int id, [FromBody] ExpenseWriteModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var dbEvent = await _dbContext.EventData.FirstOrDefaultAsync(ev => ev.Id == id);
            if (dbEvent == null) return NotFound();

            var expenseToAdd = _mapper.Map<Expense>(model);
            _dbContext.ExpenseData.Add(expenseToAdd);
            dbEvent.Expenses.Add(expenseToAdd);

            // TODO: createdatroute, test and workout
            return Ok(expenseToAdd);
        }
    }
}
