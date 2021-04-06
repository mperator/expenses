using Expenses.Domain.Common;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.ValueObjects
{
    public class Currency : ValueObject
    {
        public string Code { get; }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new Exception("Invalid code.");
            if (!(code.Length == 3)) throw new Exception("Length not invalid");

            Code = code;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
