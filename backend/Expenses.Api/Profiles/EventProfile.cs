using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using AutoMapper;

namespace Expenses.Api.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventWriteModel>().ReverseMap();
            CreateMap<Event, EventReadModel>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator.UserName))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id));
            CreateMap<Event, EventUpdateModel>().ReverseMap();
        }
    }
}
