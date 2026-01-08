using TravelShare.Models.Users;

namespace TravelShare.Models.Authentication;

public class EmailAuthenticationProvider : AuthenticationProvider
{
    private readonly Dictionary<string, (string password, User user)> _mockUsers;

    public override string ProviderName => "Email";

    public EmailAuthenticationProvider()
    {
        // Mock data for demonstration - matching test expectations
        _mockUsers = new Dictionary<string, (string, User)>
        {
            ["student@travelshare.com"] = ("password", new Student
            {
                Id = 1,
                Email = "student@travelshare.com",
                FirstName = "Marko",
                LastName = "Horvat",
                StudentId = "0246012345",
                University = "Sveučilište u Zagrebu",
                Faculty = "Fakultet elektrotehnike i računarstva",
                PhoneNumber = "+385 99 123 4567",
                ProfileImageUrl = "/images/default-avatar.png",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Preferences = new TravelPreferences
                {
                    MaxBudget = 500,
                    MinBudget = 100,
                    PreferredDestinations = new List<string> { "Barcelona", "Prague", "Budapest" },
                    PreferredTravelType = TravelType.Backpacking,
                    PreferredAccommodation = AccommodationType.Hostel
                }
            }),
            ["admin@travelshare.com"] = ("adminpass", new Administrator
            {
                Id = 2,
                Email = "admin@travelshare.com",
                FirstName = "Ana",
                LastName = "Kovač",
                Department = "User Management",
                ProfileImageUrl = "/images/default-avatar.png",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Permissions = new List<string> { "ManageUsers", "ViewReports", "ManageTrips" }
            }),
            ["ivan.novak@travelshare.com"] = ("ivanpass", new Student
            {
                Id = 3,
                Email = "ivan.novak@travelshare.com",
                FirstName = "Ivan",
                LastName = "Novak",
                StudentId = "0246098765",
                University = "Sveučilište u Splitu",
                Faculty = "Ekonomski fakultet",
                PhoneNumber = "+385 98 765 4321",
                ProfileImageUrl = "/images/default-avatar.png",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Preferences = new TravelPreferences
                {
                    MaxBudget = 800,
                    MinBudget = 200,
                    PreferredDestinations = new List<string> { "Paris", "Rome", "Amsterdam" },
                    PreferredTravelType = TravelType.Cultural,
                    PreferredAccommodation = AccommodationType.Hotel
                }
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

    protected override Task<User?> GetUserAsync(string identifier)
    {
        if (_mockUsers.TryGetValue(identifier, out var userData))
        {
            return Task.FromResult<User?>(userData.user);
        }
        return Task.FromResult<User?>(null);
    }
}