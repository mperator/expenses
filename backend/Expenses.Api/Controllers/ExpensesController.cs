using AutoMapper;
using Expenses.Api.Data;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<ActionResult> GetExpensesAsync()
        {
            
        }
    }
}
