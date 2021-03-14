using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.EntitiesOld
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public IList<Event> Events { get; set; }

        public ICollection<ExpenseUser> ExpensesUsers { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

    }
}
