using AutoMapper;
using Expenses.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Users.Queries.SearchUsersById
{
    public class SearchUsersByIdQuery : IRequest<IEnumerable<SearchUsersByIdQueryUser>>
    {
        public string Id { get; set; }
    }

    public class GetAttendeeByIdQueryHandler : IRequestHandler<SearchUsersByIdQuery, IEnumerable<SearchUsersByIdQueryUser>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAttendeeByIdQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchUsersByIdQueryUser>> Handle(SearchUsersByIdQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(null, request.Id);
            return _mapper.Map<IEnumerable<SearchUsersByIdQueryUser>>(users);
        }
    }
}
