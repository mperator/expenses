using System;

namespace Expenses.Domain.Exceptions
{
    public abstract class ValidationException : Exception
    {
        public string Code { get; }

        protected ValidationException(string code, string message) : base(message)
        {
            Code = code;
        }

        protected ValidationException(string code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
