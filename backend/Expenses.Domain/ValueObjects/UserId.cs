﻿using System;

namespace Expenses.Domain.ValueObjects
{
    public class UserId // : valueobjects
    {
        public string Id { get; }

        private UserId() {}

        public UserId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new Exception("Invelid user id.");
            Id = id;
        }
    }
}
