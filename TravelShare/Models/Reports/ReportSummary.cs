namespace TravelShare.Models.Reports
{
    public class ReportSummary
    {
        public int TripId { get; set; }
        public double TotalExpenses { get; set; }
        public List<UserBalance> UserBalances { get; set; } = new();
    }
}
