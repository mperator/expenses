using Expenses.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Services
{
    public class EventCreateModel
    {

    }

    public class EventUpdateModel
    {

    }

    public class EventReadModel
    {

    }

    public class EventService   // entry point for presentation
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }


        //    public void CreateEvent(EventModel eventModel)
        //    {
        //        // assume eventModel was validated by API validation or validate here an throw back to api

        //        // convert event model into Event Domain date

        //        // add domainmodel if create into repository

        //        // assume created
        //    }

        //    public void EventModel GetEventById()
        //    {

        //    }

        //    // domain business -> do we want to use model no!! only to communicate outside

        //    // if we ask the Warehouse management to work with order -> do we pass in order entity or id -> use of domain events??
        //}

        public EventReadModel GetEventById()
        {
            return null;
        }

        public IEnumerable<EventReadModel> GetEvents()
        {
            return null;
        }

        public IEnumerable<EventReadModel> GetEventsByFilter()
        {
            return null;
        }

        // Create
        public void CreateEvent(EventCreateModel model)
        {
            // create domain model
            // add domain model to repository.

            // save repository
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

        // Add Participants
        public void AddParticipantToEvent(int eventId, string participantId)
        {
            // get domain
            // add participant

            // save oarticipant
        }

        // Remove Participant
        public void RemoveParticipantFromEvent(int eventId, string participantId)
        {

        }
    }
}
