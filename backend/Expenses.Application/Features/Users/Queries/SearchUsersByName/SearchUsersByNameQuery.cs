using AutoMapper;
using Expenses.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Users.Queries.SearchUsersByName
{
    public class SearchUsersByNameQuery : IRequest<IEnumerable<SearchUsersByNameQueryUser>>
    {
        public string SearchText { get; set; }
    }

    public class GetAttendeeQueryHandler : IRequestHandler<SearchUsersByNameQuery, IEnumerable<SearchUsersByNameQueryUser>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAttendeeQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchUsersByNameQueryUser>> Handle(SearchUsersByNameQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(request.SearchText, null);
            return _mapper.Map<IEnumerable<SearchUsersByNameQueryUser>>(users);
        }
    }
}
