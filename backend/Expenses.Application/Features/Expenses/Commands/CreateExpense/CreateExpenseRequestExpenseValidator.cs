using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseRequestExpenseValidator : AbstractValidator<CreateExpenseRequestExpense>
    {
        public CreateExpenseRequestExpenseValidator()
        {
            RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(e => e.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(e => e.Participants).NotEmpty().Must(a => a.Count >= 2).WithMessage("Need at least two participants.");

            RuleFor(e => e.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
