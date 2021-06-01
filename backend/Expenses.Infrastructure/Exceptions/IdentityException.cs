using Expenses.Domain.Exceptions;
using System;

namespace Expenses.Infrastructure.Exceptions
{
    public class IdentityException : DomainException
    {
        public IdentityException(string message) : base(message)
        {
        }

        public IdentityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
