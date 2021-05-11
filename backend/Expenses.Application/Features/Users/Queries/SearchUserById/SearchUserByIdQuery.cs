using AutoMapper;
using Expenses.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Users.Queries.SearchUserById
{
    public class SearchUserByIdQuery : IRequest<SearchUserByIdQueryUser>
    {
        public string Id { get; set; }
    }

    public class GetAttendeeByIdQueryHandler : IRequestHandler<SearchUserByIdQuery, SearchUserByIdQueryUser>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAttendeeByIdQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<SearchUserByIdQueryUser> Handle(SearchUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindByIdAsync(request.Id);
            return _mapper.Map<SearchUserByIdQueryUser>(user);
        }
    }
}
