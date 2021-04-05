using Expenses.Domain.Entities;
using System;

namespace Expenses.Domain.ValueObjects
{
    public class Credit
    {
        public User Creditor { get; }

        public decimal Amount { get; }

        private Credit() { } // EF

        public Credit(User creditor, decimal amount)
        {
            if (creditor == null) throw new Exception("Invalid creditor id.");
            if(amount < 0.0M) throw new Exception("Amount must me greater than zero.");

            Creditor = creditor;
            Amount = amount;
        }
    }
}
