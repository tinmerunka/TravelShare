using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TravelShare.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public int ExpenseId { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }

        [Required(ErrorMessage = "Unesite broj kartice")]
        [CreditCard(ErrorMessage = "Neispravan broj kartice")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Unesite datum isteka (MM/YY)")]
        public string Expiry { get; set; }  

        [Required(ErrorMessage = "Unesite CVV")]
        public string Cvv { get; set; }
    }
}