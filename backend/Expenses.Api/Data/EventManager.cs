using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Data
{
    public class EventManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public EventManager(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



    }
}
