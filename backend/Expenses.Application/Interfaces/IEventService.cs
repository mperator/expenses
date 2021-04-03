using Expenses.Application.Models;
using System.Collections.Generic;

namespace Expenses.Application.Interfaces
{
    public interface IEventService
    {
        public EventReadModel CreateEvent(EventCreateModel model);
        public EventReadModel GetEventById(int eventId);
        public IEnumerable<EventReadModel> GetEventsByFilter();
        public void UpdateEvent(int eventId, EventUpdateModel model);
        public void DeleteEvent(int eventId);
        public void AddParticipant(int eventId, string participantId);
        public void RemoveParticipant(int eventId, string participantId);
    }
}
