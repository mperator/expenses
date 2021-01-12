using AutoMapper;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;

namespace Expenses.Api.Profiles
{
    public class ExpenseUserProfile : Profile
    {
        public ExpenseUserProfile()
        {
            CreateMap<ExpenseUser, ExpenseUserReadModel>().ReverseMap();
        }

    }
}
