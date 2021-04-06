using Expenses.Application.Common.Models;
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

        #endregion

        public static implicit operator AppUser(ApplicationUser applicationUser)
        {
            return new AppUser
            {
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                DateOfBirth = applicationUser.DateOfBirth,
                Email = applicationUser.Email,
                Id = applicationUser.Id,
                Username = applicationUser.UserName
            };
        }
    }
}
