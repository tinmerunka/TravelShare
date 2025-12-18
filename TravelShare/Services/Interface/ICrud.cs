using TravelShare.Models.Expenses;

namespace TravelShare.Services.Interface
{
    public interface IWrite<T>
    {
        Expense Add(T entity);
        bool Delete(int id);
    }

    public interface IRead<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }
}