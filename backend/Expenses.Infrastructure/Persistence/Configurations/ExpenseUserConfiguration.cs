using Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Infrastructure.Persistence.Configurations
{
    class ExpenseUserConfiguration : IEntityTypeConfiguration<ExpenseUser>
    {
        public void Configure(EntityTypeBuilder<ExpenseUser> builder)
        {
            // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
            builder.HasKey(e => new { e.ExpenseId, e.UserId });
            builder.HasOne(eu => eu.Expense)
                .WithMany(e => e.ExpensesUsers)
                .HasForeignKey(eu => eu.ExpenseId);
            builder.HasOne(eu => eu.User)
                .WithMany(e => e.ExpensesUsers)
                .HasForeignKey(eu => eu.UserId);
        }
    }
}
