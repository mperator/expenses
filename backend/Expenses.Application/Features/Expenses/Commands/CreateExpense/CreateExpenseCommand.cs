using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    // TODO TESTEN
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
            var model = request.Model;

            // TODO Implement Validation
            if (string.IsNullOrEmpty(model.Title)) throw new ValidationException();
            //if (request.Date) throw new ValidationException();

            var @event = await _context.EventData.FirstOrDefaultAsync(ev => ev.Id == request.EventId);
            if (@event == null) throw new NotFoundException("TODO");

            var user = await _userService.GetCurrentUserAsync();

            var expense = _mapper.Map<Expense>(model);
            expense.EventId = request.EventId;
            expense.Event = @event;
            expense.Issuer = user;
            expense.IssuerId = user.Id;
            expense.Currency = "EUR";

            _context.ExpenseData.Add(expense);
            @event.Expenses.Add(expense);

            _context.ExpenseUsers.AddRange(model.Participants.Select(e => new ExpenseUser
            {
                Amount = e.Amount,
                Expense = expense,
                UserId = e.Id
            }));

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            //return CreatedAtRoute(nameof(GetExpenseById), new { eventId = eventId, expenseId = expenseToAdd.Id }, _mapper.Map<ExpenseReadModel>(expenseToAdd));
            return _mapper.Map<CreateExpenseResponseExpense>(expense);
        }
    }
}
