namespace TravelShare.Models.Authentication;
public abstract class AuthenticationProvider
{
    public abstract string ProviderName { get; }
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
    protected abstract Task<Users.User?> GetUserAsync(string identifier);
    
    protected virtual async Task LogAuthenticationAsync(Users.User user)
    {
        user.LastLoginAt = DateTime.UtcNow;
        await Task.CompletedTask;
    }
}
