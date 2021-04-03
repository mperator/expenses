using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Models
{
    public class EventCreateModel
    {
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public DateTime StartTime { get; internal set; }
        public DateTime EndTime { get; internal set; }
        public string Currency { get; internal set; }
        public IEnumerable<EventCreateParticipantModel> Participants { get; internal set; }
    }

    public class EventCreateParticipantModel
    {
        public string Id { get; set; }
    }
}
