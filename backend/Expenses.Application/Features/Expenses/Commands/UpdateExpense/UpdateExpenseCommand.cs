using Expenses.Application.Common.Interfaces;
using Expenses.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseCommand : IRequest<Unit>
    {
        public int EventId { get; set; }
        public int ExpenseId { get; set; }
        public UpdateExpenseRequestExpense Model { get; set; }
    }

    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Unit>
    {
        private readonly IAppDbContext _context;

        public UpdateExpenseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.EventId);

            var expense = @event.Expenses.SingleOrDefault(e => e.Id == request.ExpenseId);
            expense.Title = request.Model.Title;
            expense.Description = request.Model.Description;

            var credit = new Credit(
                new User(request.Model.Credit.CreditorId),
                request.Model.Credit.Amount);

            var debits = request.Model.Debits
                .Select(d => new Debit(new User(d.DebitorId), d.Amount))
                .ToList();
            expense.Split(credit, debits);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
