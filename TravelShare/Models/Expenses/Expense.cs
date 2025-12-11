namespace TravelShare.Models.Expenses
{
    public class Expense
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int PaidByUserId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ExpenseShare> Shares { get; set; } = new();
    }
}
