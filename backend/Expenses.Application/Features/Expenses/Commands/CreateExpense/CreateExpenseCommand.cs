using AutoMapper;
using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommand : IRequest<CreateExpenseResponseExpense>
    {
        public int EventId { get; set; }
        public CreateExpenseRequestExpense Model { get; set; }
    }

    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, CreateExpenseResponseExpense>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateExpenseCommandHandler(IAppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<CreateExpenseResponseExpense> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.EventId);

            var creator = await _userService.GetCurrentUserAsync();

            var expense = new Expense(
                new User(creator.Id),
                request.Model.Title,
                request.Model.Description,
                request.Model.Date,
                request.Model.Currency);

            var credit = new Credit(
                new User(request.Model.Credit.CreditorId),
                request.Model.Credit.Amount);

            var debits = request.Model.Debits
                .Select(d => new Debit(new User(d.DebitorId), d.Amount))
                .ToList();
                
            expense.Split(credit, debits);

            @event.AddExpense(expense);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateExpenseResponseExpense>(expense);
        }
    }
}
