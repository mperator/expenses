using FluentValidation;

namespace Expenses.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty()
                .WithMessage(Localization.Language.EventCreateTitleRequired);

            RuleFor(e => e.Description)
                .NotEmpty()
                .WithMessage(Localization.Language.EventCreateDescriptionRequired);

            RuleFor(e => e.Currency)
                .Length(3)
                .WithMessage(Localization.Language.EventCreateCurrency);

            RuleFor(e => e.StartDate)
                .NotEmpty()
                .WithMessage(Localization.Language.EventCreateStartDate);

            RuleFor(e => e.EndDate)
                .NotEmpty()
                .WithMessage(Localization.Language.EventCreateEndDate);
        }
    }
}
