using TravelShare.Models.Users;

namespace TravelShare.Services.Interfaces;
public interface ICurrentUserService
{
    User? GetCurrentUser();
    void StoreCurrentUser(User user);
    bool IsUserLoggedIn();
    void ClearCurrentUser();
}