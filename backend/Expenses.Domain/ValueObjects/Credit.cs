using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.ValueObjects
{
    public class Credit
    {
        public UserId CreditorId { get; set; }

        public decimal Amount { get; set; }
    }
}
