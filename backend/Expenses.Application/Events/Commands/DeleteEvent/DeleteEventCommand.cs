using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteEventCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {

            var dbEvent = await _context.EventData.FirstOrDefaultAsync(ev => ev.Id == request.Id);

            //TODO: throw not found exception for user

            _context.EventData.Remove(dbEvent);
            //TODO: use try catch blog to use cancellationToken
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
