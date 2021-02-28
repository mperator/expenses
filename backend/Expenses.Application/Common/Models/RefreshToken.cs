using Microsoft.EntityFrameworkCore;
using System;

namespace Expenses.Application.Common.Models
{
    // TODO is this the right place?
    //[Owned] -> test with fluent
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
