namespace TravelShare.Models.Authentication;

/// <summary>
/// Base class for authentication providers - demonstrates polymorphism
/// </summary>
public abstract class AuthenticationProvider
{
    public abstract string ProviderName { get; }
    
    /// <summary>
    /// Template method pattern - defines the skeleton of authentication
    /// </summary>
    public async Task<AuthenticationResult> AuthenticateAsync(string identifier, string credential)
    {
        if (!await ValidateCredentialsAsync(identifier, credential))
        {
            return AuthenticationResult.Failed("Invalid credentials");
        }

        var user = await GetUserAsync(identifier);
        if (user == null)
        {
            return AuthenticationResult.Failed("User not found");
        }

        await LogAuthenticationAsync(user);
        return AuthenticationResult.Success(user);
    }

    protected abstract Task<bool> ValidateCredentialsAsync(string identifier, string credential);
    protected abstract Task<Users.UserBase?> GetUserAsync(string identifier);
    
    protected virtual async Task LogAuthenticationAsync(Users.UserBase user)
    {
        user.LastLoginAt = DateTime.UtcNow;
        await Task.CompletedTask;
    }
}
