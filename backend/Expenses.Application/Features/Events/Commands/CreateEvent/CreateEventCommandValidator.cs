using Expenses.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IAppDbContext _context;

        public CreateEventCommandValidator(IAppDbContext context)
        {
            _context = context;

            // validate incoming event
            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Title is required.");
            //FIXME: validate all props
        }
    }
}
