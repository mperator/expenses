using System;

namespace Expenses.Api.Data.Dtos
{
    public class ExpenseUpdateModel
    {
        #region Properties

        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public float Amount { get; set; }

        #endregion
    }
}
