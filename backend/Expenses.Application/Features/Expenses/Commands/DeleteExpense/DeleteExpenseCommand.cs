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

        public DeleteExpenseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.EventId);

            var expense = @event.Expenses.SingleOrDefault(e => e.Id == request.ExpenseId);
            
            // Remove from event but not from database
            @event.RemoveExpense(expense);

            // Remove from database
            _context.Expenses.Remove(expense);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
