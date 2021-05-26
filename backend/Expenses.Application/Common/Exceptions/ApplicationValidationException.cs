using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Expenses.Application.Common.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public ModelStateDictionary ModelStateDictionary { get; } 

        public ApplicationValidationException(string message, ModelStateDictionary modelStateDictionary) : base(message)
        {
            ModelStateDictionary = modelStateDictionary;
        }
    }
}
