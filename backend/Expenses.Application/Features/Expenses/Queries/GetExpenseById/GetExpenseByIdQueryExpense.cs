using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdQueryExpense : IMapFrom<Expense>
    {
        public int Id { get; set; }
        public string CreatorId{ get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }

        public GetExpenseByIdQueryExpenseCredit Credit { get; set; }
        public IEnumerable<GetExpenseByIdQueryExpenseDebit> Debits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Expense, GetExpenseByIdQueryExpense>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.Creator.Id));
        }
    }

    public class GetExpenseByIdQueryExpenseCredit : IMapFrom<Credit>
    {
        public string CreditorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Credit, GetExpenseByIdQueryExpenseCredit>()
                .ForMember(d => d.CreditorId, o => o.MapFrom(s => s.Creditor.Id));
        }
    }

    public class GetExpenseByIdQueryExpenseDebit : IMapFrom<Debit>
    {
        public string DebitorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Debit, GetExpenseByIdQueryExpenseDebit>()
                .ForMember(d => d.DebitorId, o => o.MapFrom(s => s.Debitor.Id));
        }
    }
}
