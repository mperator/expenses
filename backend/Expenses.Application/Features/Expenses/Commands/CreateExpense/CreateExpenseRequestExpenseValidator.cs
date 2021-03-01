using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseRequestExpenseValidator : AbstractValidator<CreateExpenseRequestExpense>
    {
        public CreateExpenseRequestExpenseValidator()
        {
            RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(e => e.Title).NotEmpty().WithMessage("Title is required.");
        }
    }
}
