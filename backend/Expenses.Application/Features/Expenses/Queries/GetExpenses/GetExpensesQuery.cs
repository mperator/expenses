using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesQuery : IRequest<IEnumerable<GetExpensesQueryExpense>>
    {
        public int EventId { get; set; }
    }

    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, IEnumerable<GetExpensesQueryExpense>>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetExpensesQueryHandler(IAppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetExpensesQueryExpense>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Expenses
                .AsNoTracking()
                .Where(e => EF.Property<int>(e, "EventId") == request.EventId)
                .ProjectTo<GetExpensesQueryExpense>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
