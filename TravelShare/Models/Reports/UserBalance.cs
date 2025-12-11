namespace TravelShare.Models.Reports
{
    public class UserBalance
    {
        public int UserId { get; set; }
        public double TotalPaid { get; set; }
        public double TotalShouldPay { get; set; }
        public double Balance { get; set; }
    }
}
