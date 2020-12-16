using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using AutoMapper;

namespace Expenses.Api.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventReadModel>();

            CreateMap<EventWriteModel, Event>();
            CreateMap<EventUpdateModel, Event>();
        }
    }
}
