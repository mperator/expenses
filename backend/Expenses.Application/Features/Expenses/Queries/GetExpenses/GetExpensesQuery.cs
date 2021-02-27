using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesQuery : IRequest<IEnumerable<GetExpensesExpenseModel>>
    {
        public int EventId { get; set; }
    }

    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, IEnumerable<GetExpensesExpenseModel>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetExpensesQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetExpensesExpenseModel>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var @event = await _context.EventData
                .Include(ev => ev.Expenses)
                .SingleOrDefaultAsync(ev => ev.Id == request.EventId);

            if(@event == null)
                throw new NotFoundException($"Event {request.EventId} not found.");

            return @event.Expenses == null ? 
                Enumerable.Empty<GetExpensesExpenseModel>() : @event.Expenses.Select(e => _mapper.Map<GetExpensesExpenseModel>(e));
        }
    }
}
