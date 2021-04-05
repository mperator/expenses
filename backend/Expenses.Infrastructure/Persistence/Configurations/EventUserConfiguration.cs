using Expenses.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Expenses.Infrastructure.Persistence.Configurations
//{
//    public class EventUserConfiguration : IEntityTypeConfiguration<EventUser>
//    {
//        public void Configure(EntityTypeBuilder<EventUser> builder)
//        {
//            // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
//            builder.HasKey(e => new { e.EventId, e.UserId });

//            builder.HasOne(a => a.User)
//                .WithMany()
//                .HasForeignKey(a => a.UserId);

//            builder.HasOne(x => x.Event)
//                .WithMany()
//                .HasForeignKey(eu => eu.EventId);
//        }
//    }
//}
