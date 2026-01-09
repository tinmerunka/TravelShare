namespace TravelShare.ViewModels
{
    public class ExpenseShareViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double ShareAmount { get; set; }
        public bool CanPay { get; set; }
    }
}
