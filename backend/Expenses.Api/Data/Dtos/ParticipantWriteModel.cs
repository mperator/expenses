using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data.Dtos
{
    public class ParticipantWriteModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
