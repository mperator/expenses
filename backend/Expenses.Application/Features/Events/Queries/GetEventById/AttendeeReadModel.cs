using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;

namespace Expenses.Application.Features.Events.Queries.GetEventById
{
    public class AttendeeReadModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
