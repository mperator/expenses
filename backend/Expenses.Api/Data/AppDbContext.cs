using Expenses.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Api.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        #region Constructors

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #endregion

        public DbSet<Event> EventData { get; set; } 
    }
}
