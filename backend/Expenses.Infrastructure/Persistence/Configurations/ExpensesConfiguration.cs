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

            builder.Property(p => p.Creator)
                .HasConversion(
                    v => v.Id.ToString(),
                    v => new User(v))
                .HasColumnName("CreatorId")
                .IsRequired();

            builder.OwnsOne<Credit>(e => e.Credit,
                sa =>
                {
                    sa.Property(p => p.Amount)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Amount")
                        .IsRequired();
                    sa.Property(p => p.Creditor)
                        .HasConversion(
                            v => v.Id.ToString(),
                            v => new User(v))
                        .HasColumnName("CreditorId")
                        .IsRequired();
                });

            builder.OwnsMany<Debit>(e => e.Debits,
                d =>
                {
                    d.Property(p => p.Amount)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Amount")
                        .IsRequired();
                    d.Property(p => p.Debitor)
                        .HasConversion(
                            v => v.Id.ToString(),
                            v => new User(v))
                        .HasColumnName("DebitorId")
                        .IsRequired();
                });
        }
    }
}
