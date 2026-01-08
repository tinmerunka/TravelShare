using TravelShare.Models.Users;

namespace TravelShare.Services.Interfaces;
public interface IProfileUpdateNotifier
{
    void Subscribe(IProfileUpdateObserver observer);
    void Unsubscribe(IProfileUpdateObserver observer);
}