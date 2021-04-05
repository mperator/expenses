using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
            return Unit.Value;

            //var expense = await _context.Expenses.FirstOrDefaultAsync(ex => ex.EventId == request.EventId && ex.Id == request.ExpenseId);
            //if (expense == null) throw new NotFoundException();

            //_context.Expenses.Remove(expense);
            //try
            //{
            //    await _context.SaveChangesAsync(cancellationToken);
            //}
            //catch (Exception e)
            //{
            //    new InvalidOperationException(e.Message);
            //}

            //return Unit.Value;
        }
    }
}
