using TravelShare.Models.Expenses;

namespace TravelShare.Services.FinanceMockData
{
    public class MockExpensesData : ICrud<Expense>
    {
        private static List<Expense> _expenses;

        public MockExpensesData()
        {
            _expenses = new List<Expense>()
            {
                new Expense
                {
                    Id = 1,
                    TripId = 1,
                    PaidByUserId = 1,
                    Amount = 90.00,
                    Description = "Dinner at local restaurant",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    Shares = new List<ExpenseShare>
                    {
                        new ExpenseShare { Id = 1, ExpenseId = 1, UserId = 1, ShareAmount = 30.00 },
                        new ExpenseShare { Id = 2, ExpenseId = 1, UserId = 2, ShareAmount = 0.00 },
                        new ExpenseShare { Id = 3, ExpenseId = 1, UserId = 3, ShareAmount = 30.00 },
                    }
                },new Expense
                {
                    Id = 2,
                    TripId = 1,
                    PaidByUserId = 2,
                    Amount = 120.00,
                    Description = "Train tickets",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    Shares = new List<ExpenseShare>
                    {
                        new ExpenseShare { Id = 4, ExpenseId = 2, UserId = 1, ShareAmount = 40.00 },
                        new ExpenseShare { Id = 5, ExpenseId = 2, UserId = 3, ShareAmount = 0.00 },
                        new ExpenseShare { Id = 6, ExpenseId = 2, UserId = 2, ShareAmount = 0.00 },
                    }
                },
                new Expense
                {
                    Id = 3,
                    TripId = 1,
                    PaidByUserId = 3,
                    Amount = 60.00,
                    Description = "Taxi to hotel",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Shares = new List<ExpenseShare>
                    {
                        new ExpenseShare { Id = 7, ExpenseId = 3, UserId = 1, ShareAmount = 20.00 },
                        new ExpenseShare { Id = 8, ExpenseId = 3, UserId = 1, ShareAmount = 0.00 },
                        new ExpenseShare { Id = 9, ExpenseId = 3, UserId = 1, ShareAmount = 0.00 },
                        
                    }
                }
            };

        }
        public List<Expense> GetAll() => _expenses;
        public Expense GetById(int id) => _expenses.FirstOrDefault(e => e.Id == id);
        public void Add(Expense e)
        {
            e.Id = _expenses.Any() ? _expenses.Max(x => x.Id) + 1 : 1;
            _expenses.Add(e);
        }
        public void Delete(int id)
        {
            var expense = GetById(id);
            if(expense != null)
            {
                _expenses.Remove(expense);
            }
        }
    }
}
