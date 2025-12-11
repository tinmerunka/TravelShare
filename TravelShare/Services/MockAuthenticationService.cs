using TravelShare.Models.Authentication;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;

namespace TravelShare.Services;
public class MockAuthenticationService : IAuthenticationService
{
    private readonly Dictionary<string, AuthenticationProvider> _providers;
    private User? _currentUser;

    public MockAuthenticationService()
    {
        _providers = new Dictionary<string, AuthenticationProvider>
        {
            ["email"] = new EmailAuthenticationProvider()
        };
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
    {
        // Use email authentication provider
        var provider = _providers["email"];
        var result = await provider.AuthenticateAsync(email, password);
        
        if (result.IsSuccess && result.User != null)
        {
            SetCurrentUser(result.User);
        }
        
        return result;
    }

    public Task<bool> RegisterUserAsync(Student student, string password)
    {
        // Mock registration - this would save to database
        return Task.FromResult(true);
    }

    public void SetCurrentUser(User user)
    {
        _currentUser = user;
    }

    public User? GetCurrentUser()
    {
        return _currentUser;
    }

    public void Logout()
    {
        _currentUser = null;
    }
}
