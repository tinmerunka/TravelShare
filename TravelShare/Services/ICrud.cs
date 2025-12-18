namespace TravelShare.Services.Interfaces
{
    public interface IRead<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }

    public interface IWrite<T>
    {
        T Create(T entity);
        bool Delete(int id);
    }
}