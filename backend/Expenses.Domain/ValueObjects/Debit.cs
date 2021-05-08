using Expenses.Domain.Common;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.ValueObjects
{
    public class Debit : ValueObject
    {
        public User Debitor { get; }

        public decimal Amount { get; }

        private Debit() { } // EF

        public Debit(User debitorId, decimal amount)
        {
            if (debitorId == null) throw new Exception("Invalid dbitor id.");
            if (amount < 0.0M) throw new Exception("Amount must me greater than zero.");

            Debitor = debitorId;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Debitor;
            yield return Amount;
        }
    }
}
