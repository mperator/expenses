using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Commands.CreateEvent
{
    // FIXME: should the response type really be an int??
    // Depends on return type of api. If specified NoContent nothing is needed else return object as is an id in location header.
    public class CreateEventCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; }
        public IEnumerable<string> ParticipantIds { get; set; }
    }

    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public CreateEventCommandHandler(IAppDbContext context, ICurrentUserService currentUserService,
            IUserService userService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            // TODO: Get user information or better get current user id / get current user information (App layer)
            var creator = await _userService.GetCurrentUserAsync();

            var @event = new Event(
                new User(creator.Id),
                request.Title,
                request.Description,
                request.StartDate,
                request.EndDate,
                request.Currency);

            foreach(var participantId in request.ParticipantIds)
            {
                // TODO: user service validate user exists rename userId in table particiepant to id or participantid to ship cnflicts with dbo
                @event.AddParticipant(new User(participantId));
            }

            _context.Events.Add(@event);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            
            return @event.Id;
        }
    }
}
