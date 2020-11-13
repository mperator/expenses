using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Expenses.Api.Entities
{
    public class User : IdentityUser
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }

        #endregion
    }
}
