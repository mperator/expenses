using Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Entities
{
    public class Event
    {
        private Event() { } // EF

        public Event(string title, string description, UserId creatorId)
        {

        }

        public void AddCreditor(UserId creditorId, decimal amount)  // Money
        {

        }

        public void AddDebitor(UserId debitorId, decimal amount)  // Money
        {

        }


        // contains list of participants eg user ids

        // list of expenses
    }
}
