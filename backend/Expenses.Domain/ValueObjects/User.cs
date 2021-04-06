using System;

namespace Expenses.Domain.ValueObjects
{
    public class User
    {
        public string Id { get; }

        private User() { } // EF

        public User(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new Exception("Invelid user id.");
            Id = id;
        }
    }
}
