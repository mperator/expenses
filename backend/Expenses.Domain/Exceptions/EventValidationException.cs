using System;

namespace Expenses.Domain.Exceptions
{
    public sealed class EventValidationException : ValidationException
    {
        public EventValidationException(string code, string message) : base(code, message)
        {
        }

        public EventValidationException(string code, string message, Exception innerException) : base(code, message, innerException)
        {
        }
    }
}
