using System;

namespace Expenses.Domain.Exceptions
{
    public sealed class ExpenseValidationException : DomainException
    {
        public ExpenseValidationException(string message) : base(message)
        {
        }

        public ExpenseValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
