using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using Expenses.Domain.EntitiesOld;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IMapper _mapper;

        public UpdateExpenseCommandHandler(IAppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;

            //var model = request.Model;

            //var expense = await _context.Expenses
            //    .Include(a => a.ExpenseUsers)
            //    .FirstOrDefaultAsync(ex => ex.EventId == request.EventId && ex.Id == request.ExpenseId);
            //if (expense == null) throw new NotFoundException();

            //var update = _mapper.Map<Expense>(model);
            ////TODO: make sure that only same user as creator or a user with appropriate role can change event

            //expense.Title = update.Title;
            //expense.Description = update.Description;
            //expense.Date = update.Date;
            //expense.Amount = update.Amount;

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
