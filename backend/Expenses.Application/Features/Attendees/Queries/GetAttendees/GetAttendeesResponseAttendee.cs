using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;

namespace Expenses.Application.Features.Attendees.Queries.GetAttendees
{
    public class GetAttendeesResponseAttendee : IMapFrom<User>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetAttendeesResponseAttendee>()
                .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));
        }
    }
}
