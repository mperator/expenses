using Expenses.Application.Common.Interfaces;
using System;

namespace Expenses.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
