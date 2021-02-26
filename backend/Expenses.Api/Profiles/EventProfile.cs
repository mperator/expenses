using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using AutoMapper;
using System;

namespace Expenses.Api.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            //CreateMap<Event, EventReadModel>();

            //CreateMap<EventWriteModel, Event>();
            //CreateMap<EventUpdateModel, Event>();
            //CreateMap<Event, EventUpdateModel>().ReverseMap();
            //CreateMap<DateTimeOffset, string>().ConvertUsing(dt => dt.ToString("u"));
        }
    }
}
