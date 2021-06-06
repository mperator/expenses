using Expenses.Domain.Common;
using Expenses.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.ValueObjects
{
    public class User : ValueObject
    {
        public string Id { get; }

        private User() { } // EF

        public User(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new BusinessValidationException(Localization.Language.UserInvalidId);
            Id = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
