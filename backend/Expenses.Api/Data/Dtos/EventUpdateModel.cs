using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data.Dtos
{
    public class EventUpdateModel
    {
        #region Properties

        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        #endregion
    }
}
