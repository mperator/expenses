using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Entities
{
    public class EventUser
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public string UserId { get; set; }
    }
}
