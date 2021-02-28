namespace Expenses.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseRequestExpenseParticipant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
