using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
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
    public class GetEventByIdQueryExpense : IMapFrom<Expense>
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }

        public GetEventByIdQueryExpenseCredit Credit { get; set; }
        public IEnumerable<GetEventByIdQueryExpenseDebit> Debits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Expense, GetEventByIdQueryExpense>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.Creator.Id));
        }
    }

    public class GetEventByIdQueryExpenseCredit : IMapFrom<Credit>
    {
        public string CreditorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Credit, GetEventByIdQueryExpenseCredit>()
                .ForMember(d => d.CreditorId, o => o.MapFrom(s => s.Creditor.Id));
        }
    }

    public class GetEventByIdQueryExpenseDebit : IMapFrom<Debit>
    {
        public string DebitorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Debit, GetEventByIdQueryExpenseDebit>()
                .ForMember(d => d.DebitorId, o => o.MapFrom(s => s.Debitor.Id));
        }
    }

    public class GetEventByIdQueryParticipant : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, GetEventByIdQueryParticipant>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));
        }
    }
}
