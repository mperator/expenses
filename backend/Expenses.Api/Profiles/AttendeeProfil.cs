using AutoMapper;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;

namespace Expenses.Api.Profiles
{
    public class AttendeeProfil : Profile
    {
        public AttendeeProfil()
        {
            CreateMap<User, AttendeeReadModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
