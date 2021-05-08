using System;

namespace Expenses.Domain.Exceptions
{
    public sealed class ExpenseValidationException : ValidationException
    {
        public ExpenseValidationException(string code, string message) : base(code, message)
        {
        }

        public ExpenseValidationException(string code, string message, Exception innerException) : base(code, message, innerException)
        {
        }
    }
}
