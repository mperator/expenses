namespace Expenses.Domain.EntitiesOld
{
    public class ExpenseUser
    {
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }

        public string UserId { get; set; }

        public double Amount { get; set; }
    }
}
