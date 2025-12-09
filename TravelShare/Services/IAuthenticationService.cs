using TravelShare.Models.Authentication;
using TravelShare.Models.Users;

namespace TravelShare.Services;
public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(string email, string password);
    Task<bool> RegisterUserAsync(Student student, string password);
    void SetCurrentUser(User user);
    User? GetCurrentUser();
    void Logout();
}
