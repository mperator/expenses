using System;

namespace Expenses.Domain.ValueObjects
{
    public class Debit
    {
        public UserId DebitorId { get; }

        public decimal Amount { get; }

        public Debit(UserId debitorId, decimal amount)
        {
            if (debitorId == null) throw new Exception("Invalid dbitor id.");
            if (amount < 0.0M) throw new Exception("Amount must me greater than zero.");

            DebitorId = debitorId;
            Amount = amount;
        }
    }
}
