using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Expenses.Infrastructure.Identity;
using Expenses.Application.Common.Models;
using Expenses.Infrastructure.Entities;
using Expenses.Domain.Entities;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.Property(e => e.Id).HasColumnName("Id");
            builder.HasOne<ApplicationUser>().WithOne().HasForeignKey<User>(x => x.Id);

            // https://docs.microsoft.com/en-us/archive/msdn-magazine/2018/april/data-points-ef-core-2-owned-entities-and-temporary-work-arounds link to complex type
            //builder.OwnsMany(u => u.RefreshTokens);

            //builder.HasMany<Expense>().WithOne().HasForeignKey(a => a.IssuerId);
        }
    }
}
