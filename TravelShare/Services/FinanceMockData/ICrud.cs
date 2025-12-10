namespace TravelShare.Services.FinanceMockData
{
    public interface ICrud<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Delete(int id);
    }
}