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
                    request.RuleFor(e => e.Title).NotNull().WithMessage(Localization.Language.ExpenseUpdateTitleRequired);
                    request.RuleFor(e => e.Description).NotNull().WithMessage(Localization.Language.ExpenseUpdateDescriptionRequired);

                    request.RuleFor(e => e.Credit).ChildRules(credit =>
                    {
                        credit.RuleFor(e => e.CreditorId).NotNull().WithMessage(Localization.Language.ExpenseUpdateCreditorIdRequired);
                        credit.RuleFor(e => e.Amount).GreaterThanOrEqualTo(0).WithMessage(Localization.Language.ExpenseUpdateCreditorAmountRequired);
                    });

                    request.RuleForEach(e => e.Debits).ChildRules(debit =>
                    {
                        debit.RuleFor(e => e.DebitorId).NotNull().WithMessage(Localization.Language.ExpenseUpdateDebitorIdRequired);
                        debit.RuleFor(e => e.Amount).GreaterThanOrEqualTo(0).WithMessage(Localization.Language.ExpenseUpdateDebitorAmountRequired);
                    });
                });
        }
    }
}
