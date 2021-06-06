using Expenses.Domain.Common;
using Expenses.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.ValueObjects
{
    public class Credit : ValueObject
    {
        public User Creditor { get; }

        public decimal Amount { get; }

        private Credit() { } // EF

        public Credit(User creditor, decimal amount)
        {
            if (creditor == null) throw new BusinessValidationException(Localization.Language.CreditInvalidCreditor);
            if(amount < 0.0M) throw new BusinessValidationException(Localization.Language.CreditInvalidAmount);

            Creditor = creditor;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Creditor;
            yield return Amount;
        }
    }
}
