using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Events.Queries.GetEvents
{
    public class GetEventsQuery : IRequest<List<EventReadModel>>
    {
    }

    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventReadModel>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetEventsQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EventReadModel>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            throw new Exception();

            //return new List<EventReadModel>(
            //    await _context.EventData
            //    .Include(ev => ev.Creator)
            //    .Include(ev => ev.Expenses)
            //    .Include(ev => ev.Attendees)
            //    .AsSingleQuery()
            //    .ProjectTo<EventReadModel>(_mapper.ConfigurationProvider)
            //    .ToListAsync(cancellationToken));
        }
    }
}
