using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Queries.GetEvents
{
    public class GetEventsQuery : IRequest<List<GetEventsQueryEvent>> { }

    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<GetEventsQueryEvent>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetEventsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetEventsQueryEvent>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Events
                .AsNoTracking()
                .ProjectTo<GetEventsQueryEvent>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
