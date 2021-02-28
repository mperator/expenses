using Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Expenses.Infrastructure.Identity;
using Expenses.Application.Common.Models;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // https://docs.microsoft.com/en-us/archive/msdn-magazine/2018/april/data-points-ef-core-2-owned-entities-and-temporary-work-arounds link to complex type
            builder.OwnsMany(u => u.RefreshTokens);

            builder.HasMany(a => a.Events)
                .WithOne()
                .HasForeignKey(e => e.CreatorId);

            builder.HasMany<Expense>().WithOne().HasForeignKey(a => a.IssuerId);
        }
    }
}
