namespace TravelShare.Services.Interface
{
    public interface IDataProvider<T>
    {
        IList<T> GetAllFromDataSource();
    }
}
