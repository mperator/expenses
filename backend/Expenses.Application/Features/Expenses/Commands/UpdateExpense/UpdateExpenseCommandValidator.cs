using FluentValidation;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
    {
        // https://danielwhittaker.me/2016/04/20/how-to-validate-commands-in-a-cqrs-application/
        public UpdateExpenseCommandValidator()
        {
            RuleFor(e => e.Model).NotNull().SetValidator(new UpdateExpenseRequestExpenseValidator());
        }
    }
}
