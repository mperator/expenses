using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
    public class GetEventByIdQueryExpense : IMapFrom<Expense>
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public GetEventByIdQueryExpenseCredit Credit {get; set;}
        public IEnumerable<GetEventByIdQueryExpenseDebit> Debits {get; set;}
    }
}
