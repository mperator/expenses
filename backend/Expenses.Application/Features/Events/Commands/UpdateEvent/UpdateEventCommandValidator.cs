using FluentValidation;

namespace Expenses.Application.Features.Events.Commands.CreateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty()
                .WithMessage("Title is required.");

            RuleFor(e => e.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(e => e.Currency)
                .Length(3)
                .WithMessage("Currency must be providet in ISO 4217.");

            RuleFor(e => e.StartDate)
                .NotEmpty()
                .WithMessage("Start date not set.");

            RuleFor(e => e.EndDate)
                .NotEmpty()
                .WithMessage("End date not set.");
        }
    }
}
