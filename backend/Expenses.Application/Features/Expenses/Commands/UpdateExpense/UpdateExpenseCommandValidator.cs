using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
    {
        public UpdateExpenseCommandValidator()
        {
            RuleFor(e => e.EventId).GreaterThan(0).WithMessage("No event specified");
            RuleFor(e => e.ExpenseId).GreaterThan(0).WithMessage("No expense specified");

            RuleFor(e => e.Model)
                .NotNull()
                .ChildRules(request =>
                {
                    request.RuleFor(e => e.Title).NotNull().WithMessage("Title is required.");
                    request.RuleFor(e => e.Description).NotNull().WithMessage("Description is required.");

                    request.RuleFor(e => e.Credit).ChildRules(credit =>
                    {
                        credit.RuleFor(e => e.CreditorId).NotNull().WithMessage("Debitor is required.");
                        credit.RuleFor(e => e.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount is required.");
                    });

                    request.RuleForEach(e => e.Debits).ChildRules(debit =>
                    {
                        debit.RuleFor(e => e.DebitorId).NotNull().WithMessage("Debitor is required.");
                        debit.RuleFor(e => e.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount is required.");
                    });
                });
        }
    }
}
