using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQueryEvent : IMapFrom<Event>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<GetEventByIdQueryParticipant> Participants { get; set; }
        public IEnumerable<GetEventByIdQueryExpense> Expenses { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, GetEventByIdQueryEvent>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.Creator.Id))
                .ForMember(d => d.Participants, o => o.Ignore());   // mapped in later process.
        }
    }
}
