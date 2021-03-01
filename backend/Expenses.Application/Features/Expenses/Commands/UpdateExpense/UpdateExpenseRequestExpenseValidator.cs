using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseRequestExpenseValidator : AbstractValidator<UpdateExpenseRequestExpense>
    {
        public UpdateExpenseRequestExpenseValidator()
        {
            RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(e => e.Title).NotEmpty().WithMessage("Title is required.");
        }
    }
}
