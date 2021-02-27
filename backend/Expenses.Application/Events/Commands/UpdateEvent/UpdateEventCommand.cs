using Expenses.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public IEnumerable<AttendeeWriteModel> Attendees { get; set; }

        #endregion
    }

    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;

        public UpdateEventCommandHandler(IAppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var dbEvent = await _context.EventData
                .Include(ev => ev.Creator)
                .Include(e => e.Attendees)
                .Include(ev => ev.Expenses)
                .AsSingleQuery()
                .FirstOrDefaultAsync(ev => ev.Id == request.Id);

            dbEvent.Title = request.Title;
            dbEvent.StartDate = request.StartDate;
            dbEvent.EndDate = request.EndDate;
            dbEvent.Description = request.Description;
            //dbEvent.Creator = update.Creator;
            //dbEvent.Currency = update.Currency;
            
            var currentUser = await _userService.GetCurrentUserAsync();

            foreach (var attendee in request.Attendees.Where(attendee => attendee.Id != currentUser.Id))
            {
                var foundAttendee = await _userService.FindByIdAsync(attendee.Id);
                dbEvent.Attendees.Add(foundAttendee);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
