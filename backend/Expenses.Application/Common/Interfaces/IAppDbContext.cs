using Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Event> EventData { get; set; }

        DbSet<Expense> ExpenseData { get; set; }

        DbSet<ExpenseUser> ExpenseUsers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
