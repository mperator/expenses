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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Event>().HasMany(e => e.Attendees).WithMany(a => a.Events);
            builder.Entity<Event>().HasOne(e => e.Creator);
        }

        public DbSet<Event> EventData { get; set; } 
        public DbSet<Expense> ExpenseData { get; set; }
    }
}
