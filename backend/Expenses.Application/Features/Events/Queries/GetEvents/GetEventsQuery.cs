using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Queries.GetEvents
{
    public class GetEventsQuery : IRequest<List<GetEventsQueryEvent>> { }

    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<GetEventsQueryEvent>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetEventsQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetEventsQueryEvent>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId;

            return await _context.Events
                .Where(e => e.Participants.Select(e => e.Id).Contains(userId))
                .AsNoTracking()
                .ProjectTo<GetEventsQueryEvent>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
