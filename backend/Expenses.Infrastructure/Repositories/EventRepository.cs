using Expenses.Application.Common.Interfaces;
using Expenses.Application.Interfaces;
using Expenses.Domain.Entities;
using Expenses.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IAppDbContext _context;

        public EventRepository(IAppDbContext context)
        {
            _context = context;
        }

        //public async Task<Event> CreateAsync(Event entity)
        //{
        //    _context.Events.Add(entity);
        //    await _context.SaveChangesAsync();
        //}



        public void Delete(int id)
        {
            var @event = _context.Events.Find(id);

            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
