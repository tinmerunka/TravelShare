using TravelShare.Models.Authentication;
using TravelShare.Models.Users;

namespace TravelShare.Services;

/// <summary>
/// Interface for authentication service - supports multiple authentication providers
/// </summary>
public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(string email, string password);
    Task<bool> RegisterUserAsync(Student student, string password);
    void SetCurrentUser(UserBase user);
    UserBase? GetCurrentUser();
    void Logout();
}
