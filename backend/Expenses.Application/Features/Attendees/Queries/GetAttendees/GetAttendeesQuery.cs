using AutoMapper;
using Expenses.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Attendees.Queries.GetAttendees
{
    public class GetAttendeesQuery : IRequest<IEnumerable<GetAttendeesResponseAttendee>>
    {
        public GetAttendeesRequestAttendeeFilter Filter { get; set; }
    }

    public class GetAttendeeQueryHandler : IRequestHandler<GetAttendeesQuery, IEnumerable<GetAttendeesResponseAttendee>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAttendeeQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAttendeesResponseAttendee>> Handle(GetAttendeesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(request.Filter.Name);
            return _mapper.Map<IEnumerable<GetAttendeesResponseAttendee>>(users);
        }
    }
}
