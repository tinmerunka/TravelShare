using TravelShare.Models.Expenses;
<<<<<<< Updated upstream
using TravelShare.Services.Interface;
=======
using TravelShare.Services.Interfaces;
>>>>>>> Stashed changes

namespace TravelShare.Services
{
    public class ExpensesService : IRead<Expense>, IWrite<Expense>
    {
        private readonly IDataProvider<Expense> _provider;

        public ExpensesService(IDataProvider<Expense> provider)
        {
            _provider = provider;
        }

<<<<<<< Updated upstream
        public Expense Add(Expense expense)
        {
            var list = _provider.GetAllFromDataSource();
=======

        public Expense Create(Expense expense)
        {
            var list = _provider.GetAllDataFromSource();
>>>>>>> Stashed changes
            expense.Id = list.Count > 0 ? list.Max(e => e.Id) + 1 : 1;
            list.Add(expense);
            return expense;
        }

        public bool Delete(int id)
        {
<<<<<<< Updated upstream
            var list = _provider.GetAllFromDataSource();
=======
            var list = _provider.GetAllDataFromSource();
>>>>>>> Stashed changes
            var expense = list.FirstOrDefault(e => e.Id == id);
            if (expense == null) return false;

            list.Remove(expense);
            return true;
        }

        public IEnumerable<Expense> GetAll()
        {
<<<<<<< Updated upstream
            return _provider.GetAllFromDataSource();
=======
            return _provider.GetAllDataFromSource();
>>>>>>> Stashed changes
        }

        public Expense GetById(int id)
        {
<<<<<<< Updated upstream
            return _provider.GetAllFromDataSource()
                            .FirstOrDefault(x => x.Id == id);
        }
               
=======
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

            return true;
        }
>>>>>>> Stashed changes
    }

}
