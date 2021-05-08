using Expenses.Domain.Common;
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
            if (string.IsNullOrWhiteSpace(id)) throw new Exception("Invelid user id.");
            Id = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
