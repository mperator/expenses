using System.Threading.Tasks;

namespace Expenses.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string receiver, string subject, string message);
    }
}
