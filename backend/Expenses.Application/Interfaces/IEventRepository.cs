using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces
{
    public interface IEventRepository
    {
        void Delete(int id);

        void Save();

    }
}
