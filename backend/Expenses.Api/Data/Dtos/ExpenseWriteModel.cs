using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data.Dtos
{
    public class ExpenseWriteModel
    {
        #region Properties
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float Amount { get; set; }
        public string Currency { get; set; }
        #endregion
    }
}
