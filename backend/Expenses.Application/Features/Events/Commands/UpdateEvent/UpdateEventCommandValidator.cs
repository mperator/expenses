using Expenses.Application.Features.Events.Commands.UpdateEvent;
using FluentValidation;

namespace Expenses.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty()
                .WithMessage(Localization.Language.EventUpdateTitleRequired);

            RuleFor(e => e.Description)
                .NotEmpty()
                .WithMessage(Localization.Language.EventUpdateDescriptionRequired);
        }
    }
}
