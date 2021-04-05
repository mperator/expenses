using Expenses.Domain.Entities;

namespace Expenses.Infrastructure.Entities
{
    public class EventUser
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
