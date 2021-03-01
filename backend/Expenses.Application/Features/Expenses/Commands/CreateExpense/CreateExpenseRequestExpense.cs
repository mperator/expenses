﻿using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseRequestExpense : IMapFrom<Expense>
    {
        //[Required]
        public DateTime Date { get; set; }

        //[Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public float Amount { get; set; }

        public List<CreateExpenseRequestExpenseParticipant> Participants { get; set; }
    }
}