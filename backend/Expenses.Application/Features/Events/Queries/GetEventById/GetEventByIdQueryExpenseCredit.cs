using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.ValueObjects;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
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
}
