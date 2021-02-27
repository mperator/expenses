using AutoMapper;
using Expenses.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Attendees.Queries.GetAttendees
{
    public class GetAttendeesQuery : IRequest<IEnumerable<GetAttendeesAttendeeReadModel>>
    {
        public AttendeeFilter Filter { get; set; }
    }

    public class GetAttendeeQueryHandler : IRequestHandler<GetAttendeesQuery, IEnumerable<GetAttendeesAttendeeReadModel>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAttendeeQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAttendeesAttendeeReadModel>> Handle(GetAttendeesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(request.Filter.Name);
            return _mapper.Map<IEnumerable<GetAttendeesAttendeeReadModel>>(users);
        }
    }
}
