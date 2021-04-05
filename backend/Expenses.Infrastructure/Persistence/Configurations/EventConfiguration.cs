using Expenses.Domain.Entities;
using Expenses.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title)
                .HasMaxLength(100);
            builder.Property(e => e.Description);

            builder.Property(e => e.StartDate);
            builder.Property(e => e.EndDate);
            builder.Property(e => e.Currency)
                .HasMaxLength(3);

            builder.Ignore(a => a.Expenses);

            builder.HasOne<User>(e => e.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatorId);

            // Configure many to many relationship between user and event.
            builder.HasMany<User>(a => a.Participants)
                .WithMany("Events")   // Link to private navigation property.
                .UsingEntity<EventUser>(
                    eu => eu.HasOne<User>(e => e.User).WithMany().HasForeignKey(x => x.UserId),
                    eu => eu.HasOne<Event>(e => e.Event).WithMany().HasForeignKey(x => x.EventId))
                .ToTable("EventUser")
                .HasKey(e => new { e.EventId, e.UserId });
        }
    }
}
