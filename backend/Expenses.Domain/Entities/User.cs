using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Entities
{
    // https://stackoverflow.com/questions/39771808/entity-framework-core-many-to-many-relationship-with-same-entity
    public class User
    {
        public string Id { get; }

        //// needed by EF Core
        public IList<Event> Events { get; }

        public User(string id)
        {
            Id = id;
        }
    }
}
