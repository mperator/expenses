using Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<Expense> Expenses { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
