namespace TravelShare.Models.Expenses
{
    public class ExpenseShare
    {
        public int Id { get; set; }
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public double ShareAmount { get; set; }
    }
}