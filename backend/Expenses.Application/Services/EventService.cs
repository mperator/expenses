using Expenses.Application.Interfaces;
using Expenses.Application.Models;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using System.Collections.Generic;

namespace Expenses.Application.Services
{

    
    public class EventService : IEventService  // entry point for presentation
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        // Create
        public EventReadModel CreateEvent(EventCreateModel model)
        {
            var creatorId = string.Empty; // UserService.Current

            // create domain model
            var @event = new Event(new User(creatorId), model.Title, model.Description, model.StartTime, model.EndTime, model.Currency);
            
            foreach(var participantModel in model.Participants)
            {
                var participant = new User(participantModel.Id);
                @event.AddParticipant(participant);
            }

            // add domain model to repository.

            //_repository.Add(@event);
            //_repository.Save();

            // everything is ok do return model?
            // check if event.Id is avialable

            return null;
        }

        public EventReadModel GetEventById(int eventId)
        {
            Event @event; // _reposiory.GetEventById(eventId);

            // Convenrt to read model
            // return @event;

            return null;
        }

        public IEnumerable<EventReadModel> GetEventsByFilter()
        {
            // get all by or curserbased and with filter

            return null;
        }

        // Update
        public void UpdateEvent(int eventId, EventUpdateModel model)
        {
            // get domain model from repository
            // update domain model

            // save repository
        }

        // Delete
        public void DeleteEvent(int eventId)
        {
            // working on Infrastructure direct but do not use eFcore in apllicaiton

            // _get domain enitity to evaluate if can be deleted

            _repository.Delete(eventId);
            _repository.Save();

            // get domain event
            // remove 
            // save
        }


        public void AddParticipant(int eventId, string participantId)
        {
            Event @event = null; // _repository.GetEventById(eventId);
            @event.AddParticipant(new User(participantId));

            // _repository.Update(@event);
            //_repository.Save();

            // return nocontent;
        }

        public void RemoveParticipant(int eventId, string participantId)
        {
            Event @event = null; // _repository.GetEventById(eventId);
            @event.RemoveParticipant(new User(participantId));

            // _repository.Update(@event);
            //_repository.Save();

            // return nocontent;
        }
    }
}
