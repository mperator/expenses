using Expenses.Application.Common.Interfaces;
using Expenses.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<string> ParticipantIds { get; set; }
    }

    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;

        public UpdateEventCommandHandler(IAppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.Id);

            @event.Title = request.Title;
            @event.Description = request.Description;

            // Remove participants that dont exists anymore.
            foreach (var participant in @event.Participants.Reverse())
            {
                if (participant.Id == @event.Creator.Id) continue;

                if (!request.ParticipantIds.Contains(participant.Id))
                    @event.RemoveParticipant(participant);
            }

            // Add new participants
            foreach (var participantId in request.ParticipantIds)
            {
                if (@event.Participants.FirstOrDefault(p => p.Id == participantId) == null)
                {
                    // TODO: User service validate if user exists.
                    @event.AddParticipant(new User(participantId));
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
