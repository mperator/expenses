using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Expenses.Api.Data.Dtos
{
    public class ExpenseWriteModel
    {
        #region Properties

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        public float Amount { get; set; }

        public List<ParticipantWriteModel> Participants { get; set; }

        #endregion
    }
}
