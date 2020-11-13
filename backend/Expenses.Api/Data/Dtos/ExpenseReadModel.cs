using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data.Dtos
{
    public class ExpenseReadModel
    {
        #region Properties
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public int Issuer { get; set; }
        #endregion
    }
}
