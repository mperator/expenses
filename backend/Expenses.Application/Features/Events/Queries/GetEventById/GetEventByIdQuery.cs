using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQuery : IRequest<GetEventByIdQueryEvent> 
    {
        public int Id { get; set; }
    }

    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdQueryEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetEventByIdQueryHandler(IAppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<GetEventByIdQueryEvent> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .AsNoTracking()
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.Id);

            if (@event == null)
                throw new NotFoundException();

            var mappedEvent = _mapper.Map<GetEventByIdQueryEvent>(@event);

            // Get all users
            var users = new List<AppUser>();
            foreach(var participant in @event.Participants)
            {
                users.Add(await _userService.FindByIdAsync(participant.Id));
            }

            mappedEvent.Participants = _mapper.Map<IEnumerable<GetEventByIdQueryParticipant>>(users);

            return mappedEvent;
        }
    }
}
