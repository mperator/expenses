using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Services
{
    public interface IEmailService
    {
        Task SendAsync(string receiver, string subject, string message);
    }
}
