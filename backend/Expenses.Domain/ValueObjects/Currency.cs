using System;

namespace Expenses.Domain.ValueObjects
{
    public class Currency
    {
        public string Code { get; }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new Exception("Invalid code.");
            if (!(code.Length == 3)) throw new Exception("Length not invalid");

            Code = code;
        }
    }
}
