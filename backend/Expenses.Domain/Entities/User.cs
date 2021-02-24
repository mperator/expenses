using Expenses.Domain.Common;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.Entities
{
    public class User : AuditableEntity
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public IList<Event> Events { get; set; }

        #endregion
    }
}
