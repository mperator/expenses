using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
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

            builder.Property(p => p.Creator)
                .HasConversion(
                    v => v.Id.ToString(),
                    v => new User(v))
                .HasColumnName("CreatorId")
                .IsRequired();

            builder.OwnsMany<User>(e => e.Participants,
                eu =>
                {
                    eu.ToTable("Participant");
                    eu.Property(p => p.Id)
                        .HasColumnName("UserId")
                        .IsRequired();
                });

            builder.HasMany<Expense>(e => e.Expenses)
                .WithOne()
                .HasForeignKey("EventId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
