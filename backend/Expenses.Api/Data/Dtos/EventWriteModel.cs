using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public DateTimeOffset StartDate { get; set; }
        [Required]
        public DateTimeOffset EndDate { get; set; }
        
        public IEnumerable<AttendeeWriteModel> Attendees { get; set;}

        #endregion
    }
}
