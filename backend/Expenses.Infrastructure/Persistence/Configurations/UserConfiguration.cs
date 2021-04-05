using Expenses.Domain.Entities;
using Expenses.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.Property(e => e.Id).HasColumnName("Id");
            builder.HasOne<ApplicationUser>().WithOne().HasForeignKey<User>(x => x.Id);
        }
    }
}
