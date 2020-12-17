using Expenses.Api.Entities;
using System.Collections.Generic;

namespace Expenses.Api.Data.Dtos
{
    public class EventReadModel
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string CreatorId { get; set; }
        public string Currency { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public ICollection<ExpenseReadModel> Expenses { get; set; }

        #endregion
    }
}
