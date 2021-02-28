using Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Expenses.Infrastructure.Identity;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    public class EventUserConfiguration : IEntityTypeConfiguration<EventUser>
    {
        public void Configure(EntityTypeBuilder<EventUser> builder)
        {
            // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
            builder.HasKey(e => new { e.EventId, e.UserId });
            builder.HasOne<ApplicationUser>()
                .WithMany(a => a.EventUsers)
                .HasForeignKey(a => a.UserId);
            builder.HasOne(eu => eu.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(eu => eu.EventId);
        }
    }
}
