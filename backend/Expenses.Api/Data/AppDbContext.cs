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

            // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
            builder.Entity<ExpenseUser>().HasKey(e => new { e.UserId, e.ExpenseId });
            builder.Entity<ExpenseUser>()
                .HasOne(eu => eu.Expense)
                .WithMany(e => e.ExpensesUsers)
                .HasForeignKey(eu => eu.ExpenseId);
            builder.Entity<ExpenseUser>()
                .HasOne(eu => eu.User)
                .WithMany(e => e.ExpensesUsers)
                .HasForeignKey(eu => eu.UserId);
        }

        public DbSet<Event> EventData { get; set; } 
        public DbSet<Expense> ExpenseData { get; set; }

        public DbSet<ExpenseUser> ExpenseUsers { get; set; }
    }
}
