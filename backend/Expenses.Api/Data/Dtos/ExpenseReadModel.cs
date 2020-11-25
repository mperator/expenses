using System;

namespace Expenses.Api.Data.Dtos
{
    public class ExpenseReadModel
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        //public int Issuer { get; set; }

        #endregion
    }
}
