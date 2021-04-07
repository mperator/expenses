using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesQueryExpense : IMapFrom<Expense>
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }

        public GetExpensesQueryExpenseCredit Credit { get; set; }
        public IEnumerable<GetExpensesQueryExpenseDebit> Debits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Expense, GetExpensesQueryExpense>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.Creator.Id));
        }
    }

    public class GetExpensesQueryExpenseCredit : IMapFrom<Credit>
    {
        public string CreditorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Credit, GetExpensesQueryExpenseCredit>()
                .ForMember(d => d.CreditorId, o => o.MapFrom(s => s.Creditor.Id));
        }
    }

    public class GetExpensesQueryExpenseDebit : IMapFrom<Debit>
    {
        public string DebitorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Debit, GetExpensesQueryExpenseDebit>()
                .ForMember(d => d.DebitorId, o => o.MapFrom(s => s.Debitor.Id));
        }
    }
}
