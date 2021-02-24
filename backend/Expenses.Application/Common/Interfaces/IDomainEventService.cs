using Expenses.Domain.Common;
using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
