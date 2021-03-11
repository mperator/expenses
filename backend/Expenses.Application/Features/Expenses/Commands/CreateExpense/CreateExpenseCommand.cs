using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var model = request.Model;

            var @event = await _context.Events.FirstOrDefaultAsync(ev => ev.Id == request.EventId);
            if (@event == null) throw new NotFoundException("TODO");

            var user = await _userService.GetCurrentUserAsync();

            var expense = _mapper.Map<Expense>(model);
            expense.EventId = request.EventId;
            expense.Event = @event;
            expense.IssuerId = user.Id;
            expense.Currency = "EUR";

            expense.ExpenseUsers = new List<ExpenseUser>();
            foreach(var p in model.Participants)
            {
                expense.ExpenseUsers.Add(new ExpenseUser { Expense = expense, UserId = p.Id, Amount = p.Amount });
            }

            _context.Expenses.Add(expense);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

            // Build return object.

            var response =  _mapper.Map<CreateExpenseResponseExpense>(expense);
            response.Participants = new List<CreateExpenseResponseExpenseParticipant>();

            var users = expense.ExpenseUsers;
            foreach (var expenseUser in expense.ExpenseUsers)
            {
                var participant = await _userService.FindByIdAsync(expenseUser.UserId);
                response.Participants.Add(new CreateExpenseResponseExpenseParticipant { Id = participant.Id, Name = participant.Username, Amount = expenseUser.Amount });
            }

            return response;
        }
    }
}
