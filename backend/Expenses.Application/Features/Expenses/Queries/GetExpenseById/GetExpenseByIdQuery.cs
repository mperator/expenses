using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdQuery : IRequest<GetExpenseByIdQueryExpense>
    {
        public int EventId { get; set; }
        public int ExpenseId { get; set; }
    }

    // From business point of view is it necessary to get an expense from an event?
    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, GetExpenseByIdQueryExpense>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetExpenseByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetExpenseByIdQueryExpense> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Expenses
                .AsNoTracking()
                .Where(e => EF.Property<int>(e, "EventId") == request.EventId)
                .ProjectTo<GetExpenseByIdQueryExpense>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == request.ExpenseId, cancellationToken);
        }
    }
}
