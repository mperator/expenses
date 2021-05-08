using FluentValidation;

namespace Expenses.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginCommandValidator()
        {
            RuleFor(e => e.Password).NotEmpty().WithMessage("No password provided.");
        }
    }
}
