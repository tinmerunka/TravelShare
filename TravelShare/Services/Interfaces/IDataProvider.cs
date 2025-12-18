namespace TravelShare.Services.Interfaces
{
    public interface IDataProvider<T>
    {
        IList<T> GetAllDataFromSource();
    }
}
