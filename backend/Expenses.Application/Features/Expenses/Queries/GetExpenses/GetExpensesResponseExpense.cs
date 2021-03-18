﻿using Expenses.Application.Common.Mappings;
using Expenses.Domain.EntitiesOld;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesResponseExpense : IMapFrom<Expense>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string IssuerId { get; set; }
        public IList<GetExpensesResponseExpenseParticipant> Participants { get; set; }
    }
}