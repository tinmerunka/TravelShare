using TravelShare.Models.Authentication;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;

namespace TravelShare.Services;
public class MockAuthenticationService : IAuthenticationService
{
    private readonly Dictionary<string, (string password, User user)> _mockUsers;
    private User? _currentUser;

    public MockAuthenticationService()
    {
        // Mock data for demonstration - matching the test expectations exactly
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

    public Task<AuthenticationResult> AuthenticateAsync(string email, string password)
    {
        if (_mockUsers.TryGetValue(email, out var userData) && userData.password == password)
        {
            SetCurrentUser(userData.user);
            return Task.FromResult(AuthenticationResult.Success(userData.user));
        }

        return Task.FromResult(AuthenticationResult.Failed("Invalid email or password"));
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