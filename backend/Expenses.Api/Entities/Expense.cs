using System;

namespace Expenses.Api.Entities
{
    public class Expense
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public User Issuer { get; set; }
        #endregion
    }
}
