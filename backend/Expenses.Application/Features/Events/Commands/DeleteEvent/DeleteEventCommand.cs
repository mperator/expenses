using Expenses.Application.Common.Exceptions;
using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteEventCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events
                .Include(e => e.Expenses)
                .SingleOrDefaultAsync(e => e.Id == request.Id);

            // Businessrule: Only allowed for expense creator or event creator
            if (@event.Creator.Id != _currentUserService.UserId)
                throw new ForbiddenAccessException();

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
