using System;

namespace Expenses.Domain.Exceptions
{
    public sealed class EventValidationException : DomainException
    {
        public EventValidationException(string message) : base(message)
        {
        }

        public EventValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
