using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;
using System;

namespace Expenses.Application.Features.Events.Queries.GetEvents
{
    public class ExpenseReadModel : IMapFrom<Expense>
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string IssuerId { get; set; }
        public string Issuer { get; set; }

        #endregion
    }
}
