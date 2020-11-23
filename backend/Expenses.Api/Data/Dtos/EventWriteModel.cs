using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data.Dtos
{
    public class EventWriteModel
    {
        #region Properties
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
        #endregion
    }
}
