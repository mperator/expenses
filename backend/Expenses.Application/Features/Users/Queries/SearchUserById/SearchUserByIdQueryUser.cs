using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;

namespace Expenses.Application.Features.Users.Queries.SearchUserById
{
    public class SearchUserByIdQueryUser : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
