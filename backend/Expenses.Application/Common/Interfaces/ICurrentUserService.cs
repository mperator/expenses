namespace Expenses.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        //Task<User> GetCurrentUserAsync();
    }
}
