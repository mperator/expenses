using Microsoft.AspNetCore.Identity;
using System;

namespace Expenses.Api.Entities
{
    public class User : IdentityUser
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        #endregion
    }
}
