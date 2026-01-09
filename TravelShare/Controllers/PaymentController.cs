using Microsoft.AspNetCore.Mvc;
using TravelShare.Models.Expenses;
using TravelShare.Services;
using TravelShare.Services.Decorators;
using TravelShare.ViewModels;

namespace TravelShare.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly ExpensesService _expenseService;

        public PaymentController(PaymentService paymentService, ExpensesService expenseService)
        {
            _paymentService = paymentService;
            _expenseService = expenseService;
        }

        // GET: PaymentController
        public ActionResult Index()
        {
            return View();
        }

       // GET: PaymentController/Create
        public ActionResult CreatePayment(int paymentId, int expenseId)
        {
            var expense = _expenseService.GetById(expenseId);
            if (expense == null) return NotFound();

            int NumberOfParticipants = expense.Shares.Count;

            double amountPerPerson = NumberOfParticipants > 0 ? 
                                     expense.Amount / NumberOfParticipants : 
                                     expense.Amount;
            
            var payment = new PaymentViewModel()
            {
                Amount = amountPerPerson,
                Currency = "EUR",
                CardNumber = "",
                Expiry = "",
                Cvv = ""
            };
           
            ViewBag.PaymentId = paymentId;
            ViewBag.ExpenseId = expenseId;

            return View(payment);
        }

        // POST: PaymentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePayment(PaymentViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Expiry) && !model.Expiry.Contains("/"))
                ModelState.AddModelError("Expiry", "Datum mora biti u formatu MM/YY");

            if (string.IsNullOrEmpty(model.Cvv) && (model.Cvv.Length != 3))
                ModelState.AddModelError("Cvv", "CVV mora imati 3 znamenke");

            if (!ModelState.IsValid)
                return View(model);

            var payment = new Payment.Builder()
                .SetAmount(model.Amount)
                .SetCurrency(model.Currency ?? "EUR")
                .SetCardNumber(model.CardNumber)
                .SetCvv(model.Cvv)
                .SetExpiry(model.Expiry)
                .Build();

            var userEmail = "student@travelshare.com";

            var paymentEmailDecorator = new PaymentEmailDecorator(new PaymentService(), userEmail);
            paymentEmailDecorator.Pay(payment);

            return RedirectToAction("Details", "Expense", new { id = model.ExpenseId });
        }
    }
}