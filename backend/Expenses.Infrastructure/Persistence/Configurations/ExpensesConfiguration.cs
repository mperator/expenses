using Expenses.Domain.Entities;
using Expenses.Domain.ValueObjects;
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

            builder.OwnsMany<Debit>(e => e.Debits,
                d =>
                {
                    d.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
                    d.HasOne<User>(a => a.Debitor)
                        .WithOne()
                        .HasForeignKey<Debit>("DebitorId");
                    d.HasKey("Id");
                });
        }
    }
}
