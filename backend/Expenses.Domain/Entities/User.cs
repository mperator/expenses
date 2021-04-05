using System;
using System.Collections.Generic;

namespace Expenses.Domain.Entities
{
    public class User
    {
        // Navigation property needed by EFCore5: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many
        // However EF6 allows having no navigation property: https://stackoverflow.com/questions/39771808/entity-framework-core-many-to-many-relationship-with-same-entity
        private IList<Event> Events { get; }

        public string Id { get; }

        public User(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new Exception("Invelid user id.");
            Id = id;
        }
    }
}
