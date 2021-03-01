using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        // https://danielwhittaker.me/2016/04/20/how-to-validate-commands-in-a-cqrs-application/
        public CreateExpenseCommandValidator()
        {
            RuleFor(e => e.Model).NotNull().SetValidator(new CreateExpenseRequestExpenseValidator());
        }
    }
}
