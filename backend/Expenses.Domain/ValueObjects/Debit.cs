using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.ValueObjects
{
    public class Debit
    {
        public UserId DebitorId { get; set; }

        public decimal Amount { get; set; }
    }
}
