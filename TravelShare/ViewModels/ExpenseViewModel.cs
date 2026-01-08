namespace TravelShare.ViewModels
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> PaidUsers { get; set; } = new();
        public List<string> UnpaidUsers { get; set; } = new();
        public List<ExpenseShareViewModel> Shares { get; set; } = new();
    }
}
