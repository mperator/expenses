using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.EntitiesOld;

namespace Expenses.Application.Features.Events.Queries.GetEvents
{
    public class AttendeeReadModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, AttendeeReadModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));
        }
    }
}
