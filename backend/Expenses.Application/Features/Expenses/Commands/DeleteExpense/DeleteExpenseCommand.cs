using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Commands.DeleteExpense
{
    public class DeleteExpenseCommand : IRequest
    {
        public int EventId { get; set; }
        public int ExpenseId { get; set; }
    }

    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteExpenseCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.EventId);

            var expense = @event.Expenses.SingleOrDefault(e => e.Id == request.ExpenseId);

            // Businessrule: Only allowed for expense creator or event creator
            if (@event.Creator.Id != _currentUserService.UserId &&
               expense.Creator.Id != _currentUserService.UserId)
                throw new ForbiddenAccessException();

            // Remove from event but not from database
            @event.RemoveExpense(expense);

            // Remove from database
            _context.Expenses.Remove(expense);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
