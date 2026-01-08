using System.Text.Json;
using Microsoft.AspNetCore.Http;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;

namespace TravelShare.Services;
public class SessionCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = false };

    public SessionCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession? Session => _httpContextAccessor.HttpContext?.Session;

    public User? GetCurrentUser()
    {
        var userJson = Session?.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
            return null;

        var userType = Session?.GetString("UserType");
        try
        {
            return userType switch
            {
                "Student" => JsonSerializer.Deserialize<Student>(userJson, _jsonOptions),
                "Administrator" => JsonSerializer.Deserialize<Administrator>(userJson, _jsonOptions),
                _ => null
            };
        }
        catch (JsonException)
        {
            // Handle malformed JSON gracefully
            return null;
        }
    }

    public void StoreCurrentUser(User user)
    {
        if (Session == null) return;
        var userJson = JsonSerializer.Serialize(user, user.GetType(), _jsonOptions);
        Session.SetString("CurrentUser", userJson);
        Session.SetString("UserType", user.GetUserType());
        Session.SetString("UserEmail", user.Email);
    }

    public bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(Session?.GetString("CurrentUser"));
    }

    public void ClearCurrentUser()
    {
        Session?.Remove("CurrentUser");
        Session?.Remove("UserType");
        Session?.Remove("UserEmail");
    }
}