using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Application.Features.Events.Queries.GetEventsByFilter
{
    public class GetEventsByFilterQueryEvent : IMapFrom<Event>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<string> ParticipantIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, GetEventsByFilterQueryEvent>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.Creator.Id))
                .ForMember(d => d.ParticipantIds, o => o.MapFrom(s => s.Participants.Select(p => p.Id)));
        }
    }
}
