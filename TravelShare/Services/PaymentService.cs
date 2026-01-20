using LuhnDotNet.Algorithm.Luhn;
using TravelShare.Models.Expenses;
using TravelShare.ViewModels;

namespace TravelShare.Services
{
    public class PaymentService
    {
        public virtual PaymentResult Pay(Payment payment)
        {
            if (!isCardValid(payment.CardNumber))
                return PaymentResult.Declined("Invalid card number");

            if (!isCvvValid(payment.Cvv))
                return PaymentResult.Declined("Payment failed: Invalid cvv");

            var transactionId = GenerateTransactionGuid();

            return PaymentResult.Approved(transactionId);
        }

        private static string GenerateTransactionGuid() => Guid.NewGuid().ToString();

        private static bool isCvvValid(string cvv) => !string.IsNullOrWhiteSpace(cvv) && cvv.Length == 3;

        private  static bool isCardValid(string cardNumber) => !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.IsValidLuhnNumber(); // card number koji zadovoljava Luhn 4111 1111 1111 1111
    }
}
