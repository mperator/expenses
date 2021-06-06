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
                    request.RuleFor(e => e.Title).NotNull().WithMessage(Localization.Language.ExpenseCreateTitleRequired);
                    request.RuleFor(e => e.Description).NotNull().WithMessage(Localization.Language.ExpenseCreateDescriptionRequired);
                    request.RuleFor(e => e.Date).NotNull().WithMessage(Localization.Language.ExpenseCreateDateRequired);

                    request.RuleFor(e => e.Credit).ChildRules(credit =>
                    {
                        credit.RuleFor(e => e.CreditorId).NotNull().WithMessage(Localization.Language.ExpenseCreateCreditorIdRequired);
                        credit.RuleFor(e => e.Amount).GreaterThanOrEqualTo(0).WithMessage(Localization.Language.ExpenseCreateCreditorAmountRequired);
                    });

                    request.RuleForEach(e => e.Debits).ChildRules(debit =>
                    {
                        debit.RuleFor(e => e.DebitorId).NotNull().WithMessage(Localization.Language.ExpenseCreateDebitorIdRequired);
                        debit.RuleFor(e => e.Amount).GreaterThanOrEqualTo(0).WithMessage(Localization.Language.ExpenseCreateDebitorAmountRequired );
                    });
                });
        }
    }
}
