using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdQuery : IRequest<GetExpenseByIdResponseExpense>
    {
        public int EventId { get; set; }
        public int ExpenseId { get; set; }
    }

    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, GetExpenseByIdResponseExpense>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetExpenseByIdQueryHandler(IAppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<GetExpenseByIdResponseExpense> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            return null;

            //var expense = await _context.Expenses
            //    .AsNoTracking()
            //    .Include(a => a.ExpenseUsers)
            //    .FirstOrDefaultAsync(ex => ex.EventId == request.EventId && ex.Id == request.ExpenseId);

            //Expense expenses;

            //if (expense == null)
            //    throw new NotFoundException($"No expense found for id {request.ExpenseId} on event {request.EventId}");

            //var response = _mapper.Map<GetExpenseByIdResponseExpense>(expense);
            //response.Participants = new List<GetExpenseByIdResponseExpenseParticipant>();

            //var users = expense.ExpenseUsers;
            //foreach(var expenseUser in expense.ExpenseUsers)
            //{
            //    var participant = await _userService.FindByIdAsync(expenseUser.UserId);
            //    response.Participants.Add(new GetExpenseByIdResponseExpenseParticipant { Id = participant.Id, Name = participant.Username, Amount = expenseUser.Amount });
            //}

            //return response;
        }
    }
}
