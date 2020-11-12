using System;

namespace Expenses.Api.Entities
{
    public class Event
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // FIXME: evtl User anstatt string?
        public string Creator { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion
    }
}
