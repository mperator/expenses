namespace Expenses.Domain.ValueObjects
{
    public class UserId // : valueobjects
    {
        public string Id { get; }

        public UserId(string id)
        {
            Id = id;
        }
    }
}
