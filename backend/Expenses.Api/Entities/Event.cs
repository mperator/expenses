using System;
using System.Collections.Generic;

namespace Expenses.Api.Entities
{
    public class Event
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public User Creator { get; set; }
        public string CreatorId { get; set; }

        public string Currency { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public ICollection<User> Attendees { get; set; } 
        public ICollection<Expense> Expenses { get; set; }

        #endregion
    }
}
