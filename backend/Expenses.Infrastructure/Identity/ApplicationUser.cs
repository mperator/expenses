using Expenses.Application.Common.Models;
using Expenses.Domain;
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

        //public ICollection<RefreshToken> RefreshTokens { get; set; }
        
        //public IList<Event> Events { get; set; }

        //public ICollection<ExpenseUser> ExpensesUsers { get; set; }

        //public ICollection<EventUser> EventUsers { get; set; }

        #endregion

        //public static implicit operator User(ApplicationUser applicationUser)
        //{
        //    return new User {
        //        FirstName = applicationUser.FirstName,
        //        LastName = applicationUser.LastName,
        //        DateOfBirth = applicationUser.DateOfBirth,
        //        Email = applicationUser.Email,
        //        Events = applicationUser.Events,
        //        ExpensesUsers = applicationUser.ExpensesUsers,
        //        Id = applicationUser.Id,
        //        Username = applicationUser.UserName
        //    };
        //}
    }
}
