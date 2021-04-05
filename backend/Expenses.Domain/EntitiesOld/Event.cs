using Expenses.Domain.Common;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.EntitiesOld
{
    // TODO: Domain Events
    public class Event : AuditableEntity
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string CreatorId { get; set; }

        public string Currency { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        
        //public ICollection<User> Attendees { get; set; }
        //public ICollection<Expense> Expenses { get; set; }

        //public ICollection<EventUser> Participants { get; set; }

        #endregion
    }
}
