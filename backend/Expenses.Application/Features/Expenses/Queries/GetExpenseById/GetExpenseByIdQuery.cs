using AutoMapper;
using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdQuery : IRequest<GetExpenseByIdExpenseModel>
    {
        public int EventId { get; set; }
        public int ExpenseId { get; set; }
    }

    public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, GetExpenseByIdExpenseModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetExpenseByIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetExpenseByIdExpenseModel> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var expense = await _context.ExpenseData
                .FirstOrDefaultAsync(ex => ex.EventId == request.EventId && ex.Id == request.ExpenseId);

            if (expense == null)
                throw new NotFoundException($"No expense found for id {request.ExpenseId} on event {request.EventId}");

            return _mapper.Map<GetExpenseByIdExpenseModel>(expense);
        }
    }
}
