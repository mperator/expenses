using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Entities
{
    public class EventParticipant
    {
        public string UserId { get; set; }

        public int EventId { get; set; }

        public string Grant { get; set; }
    }
}
