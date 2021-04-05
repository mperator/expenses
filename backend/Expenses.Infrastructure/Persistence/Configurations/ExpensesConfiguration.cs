using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
using Expenses.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    public class ExpensesConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title)
                .HasMaxLength(100);
            builder.Property(e => e.Description);

            builder.Property(e => e.Date);
            builder.Property(e => e.Currency)
                .HasMaxLength(3);

            builder.HasOne<User>(e => e.Creator)
                .WithMany()
                .HasForeignKey("CreatorId");

            builder.OwnsOne<Credit>(e => e.Credit,
                sa =>
                {
                    sa.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
                    sa.HasOne<User>(a => a.Creditor)
                        .WithOne()
                        .HasForeignKey<Credit>("CreditorId");
                });

            


            builder.Ignore(e => e.Debits);
            
            //builder.Ignore(a => a.Expenses);

            

            //// Configure many to many relationship between user and event.
            //builder.HasMany<User>(a => a.Participants)
            //    .WithMany("Events")   // Link to private navigation property.
            //    .UsingEntity<EventUser>(
            //        eu => eu.HasOne<User>(e => e.User).WithMany().HasForeignKey(x => x.UserId),
            //        eu => eu.HasOne<Event>(e => e.Event).WithMany().HasForeignKey(x => x.EventId))
            //    .ToTable("EventUser")
            //    .HasKey(e => new { e.EventId, e.UserId });
        }
    }
}
