using System;
using System.Collections.Generic;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseRequestExpenseCredit
    {
        public string CreditorId { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateExpenseRequestExpenseDebit
    {
        public string DebitorId { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateExpenseRequestExpense
    {
        public string Title { get; set; }
        public string Description { get; set; }
        
        public UpdateExpenseRequestExpenseCredit Credit { get; set; }
        public IEnumerable<UpdateExpenseRequestExpenseDebit> Debits { get; set; }
    }
}
