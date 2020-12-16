using System;
using System.Collections.Generic;

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
        public List<AttendeeReadModel> Attendees { get; set; }

        #endregion
    }
}
