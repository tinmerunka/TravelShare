using System.ComponentModel.DataAnnotations;

namespace TravelShare.Models.Expenses
{
    public class Expense
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int PaidByUserId { get; set; }
        [Display(Name = "Amount")]
        public double Amount { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ExpenseShare> Shares { get; set; } = new();
    }
}
