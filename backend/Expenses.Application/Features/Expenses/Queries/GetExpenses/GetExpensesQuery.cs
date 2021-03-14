using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using Expenses.Domain.EntitiesOld;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesQuery : IRequest<IEnumerable<GetExpensesResponseExpense>>
    {
        public int EventId { get; set; }
    }

    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, IEnumerable<GetExpensesResponseExpense>>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetExpensesQueryHandler(IAppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetExpensesResponseExpense>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .AsNoTracking()
                .Include(ev => ev.Expenses)
                    .ThenInclude(ex => ex.ExpenseUsers)
                .SingleOrDefaultAsync(ev => ev.Id == request.EventId);

            if(@event == null)
                throw new NotFoundException($"Event {request.EventId} not found.");

            // get all users distinct
            if(@event.Expenses == null)
            {
                return Enumerable.Empty<GetExpensesResponseExpense>();
            }
            else
            {
                var userIds = @event.Expenses.SelectMany(a => a.ExpenseUsers).Select(a => a.UserId);
                var users = new List<User>();

                foreach (var id in userIds.Distinct())
                    users.Add(await _userService.FindByIdAsync(id));


                var result = new List<GetExpensesResponseExpense>();
                foreach(var expense in @event.Expenses)
                {
                    var temp = _mapper.Map<GetExpensesResponseExpense>(expense);
                    temp.Participants = expense.ExpenseUsers.Select(e => new GetExpensesResponseExpenseParticipant
                    {
                        Id = e.UserId,
                        Name = users.Where(u => u.Id == e.UserId).SingleOrDefault()?.Username,
                        Amount = e.Amount
                    }).ToList();

                    result.Add(temp);
                }

                return result;
            }
        }
    }
}
