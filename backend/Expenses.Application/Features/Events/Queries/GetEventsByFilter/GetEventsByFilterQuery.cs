using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Queries.GetEventsByFilter
{
    public class GetEventsByFilterQuery : QueryStringParameter, IRequest<PagedList<GetEventsByFilterQueryEvent>>
    {
        public string Text { get; set; }
    }

    public class GetEventsQueryHandler : IRequestHandler<GetEventsByFilterQuery, PagedList<GetEventsByFilterQueryEvent>>
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

        public async Task<PagedList<GetEventsByFilterQueryEvent>> Handle(GetEventsByFilterQuery request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId;

            var query = _context.Events
                .AsNoTracking()
                .Where(e => e.Participants.Select(e => e.Id).Contains(userId));

            if (request.Text != null)
                query = query.Where(e => e.Title.Contains(request.Text) || e.Description.Contains(request.Text));
            query = query.OrderBy(o => o.Id);

            // count total
            var count = query.Count();

            // apply paging
            query = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize);

            // get modified.
            var items =  await query
                .ProjectTo<GetEventsByFilterQueryEvent>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedList<GetEventsByFilterQueryEvent>(items, request, count);
        }
    }
}
