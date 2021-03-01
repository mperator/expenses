using AutoMapper;
using Expenses.Application.Common.Mappings;
using Expenses.Domain.Entities;

namespace Expenses.Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseResponseExpenseUser : IMapFrom<ExpenseUser>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ExpenseId { get; set; }
        public double Amount { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<ExpenseUser, CreateExpenseResponseExpenseUser> ()
        //        .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Username));
        //}
    }
}
