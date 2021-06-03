using Expenses.Domain.Common;
using Expenses.Domain.Exceptions;
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
            if (debitorId == null) throw new BusinessValidationException(Localization.Language.DebitInvalidDebitor);
            if (amount < 0.0M) throw new BusinessValidationException(Localization.Language.DebitInvalidAmount);

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
