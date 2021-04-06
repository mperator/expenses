using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQuery : IRequest<EventReadModel> 
    {
        public int Id { get; set; }
    }

    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventReadModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetEventByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventReadModel> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.Exception();

            //return await _context.EventData
            //    .Include(ev => ev.Creator)
            //    .Include(e => e.Attendees)
            //    .Include(ev => ev.Expenses)
            //    .AsSingleQuery()
            //    .ProjectTo<EventReadModel>(_mapper.ConfigurationProvider)
            //    .SingleOrDefaultAsync(ev => ev.Id == request.Id);
        }
    }
}
