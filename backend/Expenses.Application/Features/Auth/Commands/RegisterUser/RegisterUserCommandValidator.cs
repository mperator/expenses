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
                    model.RuleFor(e => e.FirstName).NotEmpty().WithMessage(Localization.Language.RegisterNoFirstName);
                    model.RuleFor(e => e.LastName).NotEmpty().WithMessage(Localization.Language.RegisterNoLastName);
                    model.RuleFor(e => e.Username).NotEmpty().WithMessage(Localization.Language.RegisterNoUsername);
                    model.RuleFor(e => e.Email).NotEmpty().EmailAddress().WithMessage(Localization.Language.RegisterInvalidEmail);
                    model.RuleFor(e => e.Password).NotEmpty().WithMessage(Localization.Language.RegisterInvalidPassword);
                });
        }
    }
}
