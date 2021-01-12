namespace Expenses.Api.Data.Dtos
{
    public class ExpenseUserReadModel
    {
        public string UserId { get; set; }
        public int ExpenseId { get; set; }
        public double Amount { get; set; }
    }
}
