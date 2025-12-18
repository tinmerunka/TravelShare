using Microsoft.AspNetCore.Mvc;
using TravelShare.Models.Expenses;
using TravelShare.Services;
<<<<<<< Updated upstream
using TravelShare.Services.Interface;
=======
using TravelShare.Services.FinanceMockData;
using TravelShare.Services.Interfaces;
>>>>>>> Stashed changes
using TravelShare.ViewModels;

namespace TravelShare.Controllers
{
    public class ExpenseController : Controller
    {
<<<<<<< Updated upstream
        private readonly IWrite<Expense> _writeService ;
        private readonly IRead<Expense> _readService;
        private readonly IUserService _userService;

        public ExpenseController(IWrite<Expense> writeService, IRead<Expense> readService, IUserService userService)
        {
            _writeService = writeService;
            _readService = readService;
            _userService = userService;
=======
        private readonly IRead<Expense> _readService;
        private readonly IWrite<Expense> _writeService;
        private readonly IUserService _usersService;

        public ExpenseController(IRead<Expense> readService, IWrite<Expense> writeService, IUserService usersService)
        {
            _readService = readService;
            _writeService = writeService;
            _usersService = usersService;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            var allUsers = await _userService.GetAllUsersAsync();
=======
            var allUsers = await _usersService.GetAllUsersAsync();
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            var users = await _userService.GetAllUsersAsync();
=======
            var users = await _usersService.GetAllUsersAsync();
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            var allUsers = await _userService.GetAllUsersAsync();
=======
            var allUsers = await _usersService.GetAllUsersAsync();
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            _writeService.Add(model);
=======
            _writeService.Create(model);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            var allUsers = await _userService.GetAllUsersAsync();
=======
            var allUsers = await _usersService.GetAllUsersAsync();
>>>>>>> Stashed changes

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
