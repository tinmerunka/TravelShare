using TravelShare.Models.Expenses;
using TravelShare.Services.Interface;

namespace TravelShare.Services
{
    public class ExpensesService : IRead<Expense>, IWrite<Expense>
    {
        private readonly IDataProvider<Expense> _provider;

        public ExpensesService(IDataProvider<Expense> provider)
        {
            _provider = provider;
        }

        public Expense Add(Expense expense)
        {
            var list = _provider.GetAllFromDataSource();
            expense.Id = list.Count > 0 ? list.Max(e => e.Id) + 1 : 1;
            list.Add(expense);
            return expense;
        }

        public bool Delete(int id)
        {
            var list = _provider.GetAllFromDataSource();
            var expense = list.FirstOrDefault(e => e.Id == id);
            if (expense == null) return false;

            list.Remove(expense);
            return true;
        }

        public IEnumerable<Expense> GetAll()
        {
            return _provider.GetAllFromDataSource();
        }

        public Expense GetById(int id)
        {
            return _provider.GetAllFromDataSource()
                            .FirstOrDefault(x => x.Id == id);
        }
               
    }

}
