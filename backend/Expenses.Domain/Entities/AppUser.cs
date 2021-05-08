using System;

namespace Expenses.Domain.Entities
{
    public class AppUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
