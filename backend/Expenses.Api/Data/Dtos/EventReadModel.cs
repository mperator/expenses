using System.Collections.Generic;
using System;

namespace Expenses.Api.Data.Dtos
{
    public class EventReadModel
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AttendeeReadModel Creator { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public ICollection<AttendeeReadModel> Attendees { get; set; }
        public ICollection<ExpenseReadModel> Expenses { get; set; }

        #endregion
    }
}
