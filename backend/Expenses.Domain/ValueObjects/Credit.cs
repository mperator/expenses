using System;

namespace Expenses.Domain.ValueObjects
{
    public class Credit
    {
        public UserId CreditorId { get; }

        public decimal Amount { get; }

        public Credit(UserId creditorId, decimal amount)
        {
            if (creditorId == null) throw new Exception("Invalid creditor id.");
            if(amount < 0.0M) throw new Exception("Amount must me greater than zero.");

            CreditorId = creditorId;
            Amount = amount;
        }
    }
}
