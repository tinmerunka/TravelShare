namespace TravelShare.ViewModels
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }

        public static PaymentResult Approved(string guid) =>
            new() { Success = true, Status = "APPROVED", TransactionId = guid };
        public static PaymentResult Declined(string message) =>
            new() { Success = false, Status = "DECLINED", Message = message };
    }
}
