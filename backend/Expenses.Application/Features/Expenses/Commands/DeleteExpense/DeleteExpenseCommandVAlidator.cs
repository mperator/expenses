using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.DeleteExpense
{
    public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
    {
        public DeleteExpenseCommandValidator()
        {
            RuleFor(e => e.EventId).NotEmpty();
            RuleFor(e => e.ExpenseId).NotEmpty();
        }
    }
}
