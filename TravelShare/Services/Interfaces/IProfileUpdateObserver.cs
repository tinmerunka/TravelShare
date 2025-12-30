using TravelShare.Models.Users;

namespace TravelShare.Services.Interfaces;
public interface IProfileUpdateObserver
{
    Task OnProfileUpdatedAsync(User user);
}