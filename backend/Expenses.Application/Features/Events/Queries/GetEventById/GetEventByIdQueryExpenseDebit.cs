using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.ValueObjects;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQueryExpenseDebit : IMapFrom<Debit>
    {
        public string DebitorId { get; }
        public decimal Amount { get; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Debit, GetEventByIdQueryExpenseDebit>()
                .ForMember(d => d.DebitorId, o => o.MapFrom(s => s.Debitor.Id));
        }
    }
}
