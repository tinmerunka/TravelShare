using TravelShare.Models.Expenses;
using TravelShare.ViewModels;

namespace TravelShare.Services.Decorators
{
    public class PaymentEmailDecorator : PaymentService
    {
        private readonly PaymentService _paymentService;
        private readonly string _userEmail;
        public PaymentEmailDecorator(PaymentService paymentService, string userEmail)
        {
            _paymentService = paymentService;
            _userEmail = userEmail;
        }

        public override PaymentResult Pay(Payment payment)
        {
            var paymentResult = _paymentService.Pay(payment);
            SendPaymentEmail(payment);
            return paymentResult;
        }

        private void SendPaymentEmail(Payment payment)
        {
            Console.WriteLine($"[EMAIL] Poslan mail na {_userEmail}: Plaćanje {payment.Amount} {payment.Currency} je uspješno provedeno.");
        }
    }
}
