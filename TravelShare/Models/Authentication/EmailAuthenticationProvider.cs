using TravelShare.Models.Users;

namespace TravelShare.Models.Authentication;

/// <summary>
/// Email/Password authentication provider - inherits from AuthenticationProvider
/// </summary>
public class EmailAuthenticationProvider : AuthenticationProvider
{
    private readonly Dictionary<string, (string password, UserBase user)> _mockUsers;

    public override string ProviderName => "Email";

    public EmailAuthenticationProvider()
    {
        // Mock data for demonstration
        _mockUsers = new Dictionary<string, (string, UserBase)>
        {
            ["student@travelshare.com"] = ("password123", new Student
            {
                Id = 1,
                Email = "student@travelshare.com",
                FirstName = "Marko",
                LastName = "Horvat",
                StudentId = "0246012345",
                University = "Sveuèilište u Zagrebu",
                Faculty = "Fakultet elektrotehnike i raèunarstva",
                PhoneNumber = "+385 99 123 4567",
                ProfileImageUrl = "/images/default-avatar.png",
                Preferences = new TravelPreferences
                {
                    MaxBudget = 500,
                    MinBudget = 100,
                    PreferredDestinations = new List<string> { "Barcelona", "Prague", "Budapest" },
                    PreferredTravelType = TravelType.Backpacking,
                    PreferredAccommodation = AccommodationType.Hostel
                }
            }),
            ["admin@travelshare.com"] = ("admin123", new Administrator
            {
                Id = 2,
                Email = "admin@travelshare.com",
                FirstName = "Ana",
                LastName = "Kovaè",
                Department = "User Management",
                ProfileImageUrl = "/images/default-avatar.png",
                Permissions = new List<string> { "ManageUsers", "ViewReports", "ManageTrips" }
            })
        };
    }

    protected override Task<bool> ValidateCredentialsAsync(string identifier, string credential)
    {
        if (_mockUsers.TryGetValue(identifier, out var userData))
        {
            return Task.FromResult(userData.password == credential);
        }
        return Task.FromResult(false);
    }

    protected override Task<UserBase?> GetUserAsync(string identifier)
    {
        if (_mockUsers.TryGetValue(identifier, out var userData))
        {
            return Task.FromResult<UserBase?>(userData.user);
        }
        return Task.FromResult<UserBase?>(null);
    }
}
