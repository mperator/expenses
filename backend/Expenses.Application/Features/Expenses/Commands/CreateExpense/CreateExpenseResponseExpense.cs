using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseResponseExpenseCredit : IMapFrom<Credit>
    {
        public string CreditorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Credit, CreateExpenseResponseExpenseCredit>()
                .ForMember(d => d.CreditorId, o => o.MapFrom(s => s.Creditor.Id)).ReverseMap();
        }
    }

    public class CreateExpenseResponseExpenseDebit : IMapFrom<Debit>
    {
        public string DebitorId { get; set; }
        public decimal Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Debit, CreateExpenseResponseExpenseDebit>()
                .ForMember(d => d.DebitorId, o => o.MapFrom(s => s.Debitor.Id));
        }
    }

    public class CreateExpenseResponseExpense : IMapFrom<Expense>
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public CreateExpenseResponseExpenseCredit Credit { get; set; }
        public IEnumerable<CreateExpenseResponseExpenseDebit> Debits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Expense, CreateExpenseResponseExpense>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.Creator.Id));
        }
    }
}
