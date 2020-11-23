using System;
using System.ComponentModel.DataAnnotations;

namespace Expenses.Api.Data.Dtos
{
    public class ExpenseWriteModel
    {
        #region Properties

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public float Amount { get; set; }

        #endregion
    }
}
