using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseRequestExpenseCredit
    {
        public string CreditorId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateExpenseRequestExpenseDebit
    {
        public string DebitorId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateExpenseRequestExpense
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public CreateExpenseRequestExpenseCredit Credit { get; set; }
        public IEnumerable<CreateExpenseRequestExpenseDebit> Debits { get; set; }
    }
}
