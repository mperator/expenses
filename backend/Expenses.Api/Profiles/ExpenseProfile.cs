using AutoMapper;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseReadModel>().ReverseMap();
            CreateMap<Expense, ExpenseWriteModel>().ReverseMap();
            CreateMap<Expense, ExpenseUpdateModel>().ReverseMap();
        }
    }
}
