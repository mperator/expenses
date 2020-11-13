using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Expenses.Api.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventWriteModel>().ReverseMap();
            CreateMap<Event, EventReadModel>().ReverseMap();
            CreateMap<Event, EventUpdateModel>().ReverseMap();
        }
    }

}
