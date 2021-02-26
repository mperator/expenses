using Expenses.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Expenses.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public IList<Event> Events { get; set; }

        public ICollection<ExpenseUser> ExpensesUsers { get; set; }

        #endregion
    }
}
