using Expenses.Domain.Common;
using Expenses.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.ValueObjects
{
    public class Currency : ValueObject
    {
        public string Code { get; }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new BusinessValidationException(Localization.Language.CurrencyInvalidCode);
            if (!(code.Length == 3)) throw new BusinessValidationException(Localization.Language.CurrencyInvalidLength);

            Code = code;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
