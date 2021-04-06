using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        // https://danielwhittaker.me/2016/04/20/how-to-validate-commands-in-a-cqrs-application/
        public CreateExpenseCommandValidator()
        {
            RuleFor(e => e.EventId).GreaterThan(0).WithMessage("No event specified.");
            RuleFor(e => e.Model)
                .NotNull()
                .ChildRules(request =>
                {
                    request.RuleFor(e => e.Title).NotNull().WithMessage("Title is required.");
                    request.RuleFor(e => e.Description).NotNull().WithMessage("Description is required.");
                    request.RuleFor(e => e.Date).NotNull().WithMessage("Date is required.");

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
