using TravelShare.Models.Users;

namespace TravelShare.Models.Authentication;

public class AuthenticationResult
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public UserBase? User { get; private set; }

    private AuthenticationResult() { }

    public static AuthenticationResult Success(UserBase user)
    {
        return new AuthenticationResult
        {
            IsSuccess = true,
            User = user
        };
    }

    public static AuthenticationResult Failed(string errorMessage)
    {
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
