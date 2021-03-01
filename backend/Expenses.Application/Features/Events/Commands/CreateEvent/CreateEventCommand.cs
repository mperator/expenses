﻿using Expenses.Application.Common.Interfaces;
using Expenses.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Features.Events.Commands.CreateEvent
{
    // FIXME: should the response type really be an int??
    public class CreateEventCommand : IRequest<int>
    {
        #region Properties

        public string Title { get; set; }

        public string Description { get; set; }

        public string Currency { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public IEnumerable<AttendeeWriteModel> Attendees { get; set; }

        #endregion
    }

    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, int>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public CreateEventCommandHandler(IAppDbContext context, ICurrentUserService currentUserService,
            IUserService userService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUserAsync();

            Event savedEvent = new Event
            {
                Title = request.Title,
                Description = request.Description,
                CreatorId = user.Id,
                Currency = "Euro",
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            savedEvent.Participants = new List<EventUser>();
            savedEvent.Participants.Add(new EventUser { Event = savedEvent, UserId = user.Id });
            ////FIXME:
            foreach (var a in request.Attendees)
            {
                savedEvent.Participants.Add(new EventUser { Event = savedEvent, UserId = a.Id });
            }

            _context.Events.Add(savedEvent);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            //FIXME:
            //return CreatedAtRoute(nameof(GetEventByIdAsync), new { id = savedEvent.Id }, _mapper.Map<EventReadModel>(savedEvent));
            return savedEvent.Id;
        }
    }
}