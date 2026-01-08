using TravelShare.Models.Expenses;
using TravelShare.Services.Interfaces;

namespace TravelShare.Services
{
    public class ExpensesService : IRead<Expense>, IWrite<Expense>
    {
        private readonly IDataProvider<Expense> _provider;

        public ExpensesService(IDataProvider<Expense> provider)
        {
            _provider = provider;
        }

        public Expense Create(Expense expense)
        {
            var list = _provider.GetAllDataFromSource();

            expense.Id = list.Count > 0 ? list.Max(e => e.Id) + 1 : 1;
            list.Add(expense);
            return expense;
        }

        public bool Delete(int id)
        {

            var list = _provider.GetAllDataFromSource();

            var expense = list.FirstOrDefault(e => e.Id == id);
            if (expense == null) return false;

            list.Remove(expense);
            return true;
        }

        public IEnumerable<Expense> GetAll()
        {
            return _provider.GetAllDataFromSource();
        }

        public Expense GetById(int id)
        {

            return _provider.GetAllDataFromSource()
                            .FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Expense expense)
        {
            var list = _provider.GetAllDataFromSource();
            var existing = list.FirstOrDefault(e => e.Id == expense.Id);

            if (existing == null) return false;

            list.Remove(existing);
            list.Add(expense);
            existing.Amount = expense.Amount;
            existing.Description = expense.Description;
            existing.TripId = expense.TripId;
            existing.PaidByUserId = expense.PaidByUserId;
            existing.CreatedAt = expense.CreatedAt;

           
            existing.Shares = expense.Shares;
            return true;
        }
    }
}
