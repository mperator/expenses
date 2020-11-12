using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
