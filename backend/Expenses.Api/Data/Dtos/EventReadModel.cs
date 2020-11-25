using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data.Dtos
{
    //TODO: how to map username to creator?
    public class EventReadModel
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Currency { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        #endregion
    }
}
