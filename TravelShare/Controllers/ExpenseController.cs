using Microsoft.AspNetCore.Mvc;
using TravelShare.Models.Expenses;
using TravelShare.Services;
using TravelShare.Services.Interface;
using TravelShare.ViewModels;

namespace TravelShare.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IWrite<Expense> _writeService ;
        private readonly IRead<Expense> _readService;
        private readonly IUserService _userService;

        public ExpenseController(IWrite<Expense> writeService, IRead<Expense> readService, IUserService userService)
        {
            _writeService = writeService;
            _readService = readService;
            _userService = userService;
        }


        // GET: ExpensesController
        public ActionResult Index()
        {
            var allExpenses = _readService.GetAll();
            return View(allExpenses);
        }

        // GET: ExpensesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
        
            var expense = _readService.GetById(id);
            if (expense == null) return NotFound();

            var allUsers = await _userService.GetAllUsersAsync();
            var paidByUser = allUsers.FirstOrDefault(u => u.Id == expense.PaidByUserId);

            var shares = expense.Shares ?? new List<ExpenseShare>();

            var vm = new ExpenseViewModel
            {
                Id = expense.Id,
                TripId = expense.TripId,
                FirstName = paidByUser?.FirstName ?? "Unknown",
                LastName = paidByUser?.LastName ?? "User",
                Amount = expense.Amount,
                Description = expense.Description,
                CreatedAt = expense.CreatedAt,
                Shares = shares.Select(s =>
                {
                    var user = allUsers.FirstOrDefault(u => u.Id == s.UserId);
                    return new ExpenseShareViewModel
                    {
                        FirstName = user?.FirstName ?? "Unknown",
                        LastName = user?.LastName ?? "User",
                        ShareAmount = s.ShareAmount
                    };
                }).ToList()
            };

            return View(vm);
        }

        // GET: ExpensesController/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var users = await _userService.GetAllUsersAsync();
            ViewBag.Users = users;


            var maxTripId = _readService.GetAll().Any() ? _readService.GetAll().Max(e => e.TripId) : 0;
            ViewBag.NextTripId = maxTripId + 1;

            return View();
        }

        // POST: ExpensesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Expense model, List<int> paidUserIds)
        {
            if (!ModelState.IsValid)
                return View(model);

            var allUsers = await _userService.GetAllUsersAsync();
            double expectedShare = model.Amount / allUsers.Count();

            model.Shares = new List<ExpenseShare>();
            int nextShareId = _readService.GetAll().SelectMany(e => e.Shares).DefaultIfEmpty().Max(s => s?.Id ?? 0) + 1;

            foreach (var user in allUsers)
            {
                bool isPaid = paidUserIds != null && paidUserIds.Contains(user.Id);
                model.Shares.Add(new ExpenseShare
                {
                    Id = nextShareId++,
                    UserId = user.Id,
                    ExpenseId = model.Id,
                    ShareAmount = isPaid ? expectedShare : -expectedShare
                });
            }

            _writeService.Add(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: ExpensesController/Delete/5
        public ActionResult Delete(int id)
        {
            var expense = _readService.GetById(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // POST: ExpensesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var expense = _readService.GetById(id);
            if (expense == null)
                return NotFound();

            _writeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: ShareCosts
        public async Task<ActionResult> ShareCosts()
        {
            double totalAmount = 550.00;
            var allUsers = await _userService.GetAllUsersAsync();

            var shares = new List<ExpenseShare>
            {
                new ExpenseShare { Id = 1, UserId = 1, ExpenseId = 1, ShareAmount = 300 },
                new ExpenseShare { Id = 2, UserId = 3, ExpenseId = 1, ShareAmount = 200 },
            };

            var sharesWithNames = shares.Select(s =>
            {
                var user = allUsers.FirstOrDefault(u => u.Id == s.UserId);
                return new ExpenseShareViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ShareAmount = s.ShareAmount
                };
            }).ToList();

            ViewBag.TotalAmount = totalAmount;
            ViewBag.ParticipantsCount = shares.Count;
            ViewBag.Shares = shares;

            return View(sharesWithNames);
        }
    }
}
