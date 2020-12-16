using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Entities
{
    // https://stackoverflow.com/questions/7050404/create-code-first-many-to-many-with-additional-fields-in-association-table
    public class ExpenseUser
    {
        public string UserId { get; set; }
        public User User{ get; set; }

        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }

        public double Amount { get; set; }
    }
}
