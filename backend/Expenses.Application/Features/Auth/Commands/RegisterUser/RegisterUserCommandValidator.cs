using FluentValidation;

namespace Expenses.Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(e => e.ConfirmationLink).NotEmpty().WithMessage("Internal error");
            RuleFor(e => e.Model)
                .NotNull()
                .ChildRules(model =>
                {
                    model.RuleFor(e => e.FirstName).NotEmpty().WithMessage("No first name provided.");
                    model.RuleFor(e => e.LastName).NotEmpty().WithMessage("No last name provided.");
                    model.RuleFor(e => e.Username).NotEmpty().WithMessage("Username name provided.");
                    model.RuleFor(e => e.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email.");
                    model.RuleFor(e => e.Password).NotEmpty().WithMessage("Invalid password.");
                });
        }
    }
}
