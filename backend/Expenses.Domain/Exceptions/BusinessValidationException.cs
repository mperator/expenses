using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Exceptions
{
    public class BusinessValidationException : DomainException
    {
        public BusinessValidationException(string message) : base(message)
        {
        }
    }
}
