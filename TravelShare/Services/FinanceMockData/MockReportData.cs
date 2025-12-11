using TravelShare.Models.Reports;

namespace TravelShare.Services.FinanceMockData
{
    public class MockReportData : ICrud<ReportSummary>
    {
        private readonly List<ReportSummary> _reports;
        public MockReportData()
        {
            _reports = new List<ReportSummary>()
            {
                new ReportSummary
                {
                    TripId = 1,
                    TotalExpenses = 270.00,
                    UserBalances = new List<UserBalance>
                    {
                        new UserBalance { UserId = 1, TotalPaid = 90.00, TotalShouldPay = 90.00 },
                        new UserBalance { UserId = 2, TotalPaid = 120.00, TotalShouldPay = 90.00 },
                        new UserBalance { UserId = 3, TotalPaid = 60.00, TotalShouldPay = 90.00},
                    }
                }
            };
        }
        public List<ReportSummary> GetAll() => _reports;
        public ReportSummary GetById(int id) => _reports.FirstOrDefault(r => r.TripId == id);
        public void Add(ReportSummary r)
        {
            r.TripId = _reports.Any() ? _reports.Last().TripId + 1 : 1;
            _reports.Add(r);
        }
        public void Delete(int id)
        {
            var report = GetById(id);
            if (report != null)
                _reports.Remove(report);
        }
    }
}
