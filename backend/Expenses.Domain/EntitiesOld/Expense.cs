using Expenses.Domain.Common;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.EntitiesOld
{
    public class Expense : AuditableEntity
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string IssuerId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        
        //FIXME: wie soll das besser abgebildet werden, ausserhalb von domain?
        public ICollection<ExpenseUser> ExpenseUsers { get; set; }

        #endregion
    }
}
