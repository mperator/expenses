﻿using Expenses.Application.Common.Mappings;
using Expenses.Domain.EntitiesOld;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseRequestExpense : IMapFrom<Expense>
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}