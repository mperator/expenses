using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;

namespace Expenses.Application.Features.Expenses.Queries.GetExpenseById
{
    public class GetExpenseByIdResponseExpenseUser : IMapFrom<ExpenseUser>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ExpenseId { get; set; }
        public double Amount { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<ExpenseUser, GetExpenseByIdExpenseUserModel > ()
        //        .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Username));
        //}
    }
}
