using TravelShare.Models.Users;

namespace TravelShare.Services;

/// <summary>
/// Mock implementation of user service for demonstration purposes
/// </summary>
public class MockUserService : IUserService
{
    private readonly List<UserBase> _users;

    public MockUserService()
    {
        _users = new List<UserBase>
        {
            new Student
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
                CreatedAt = DateTime.UtcNow.AddMonths(-3),
                LastLoginAt = DateTime.UtcNow,
                Preferences = new TravelPreferences
                {
                    MaxBudget = 500,
                    MinBudget = 100,
                    PreferredDestinations = new List<string> { "Barcelona", "Prague", "Budapest" },
                    PreferredTravelType = TravelType.Backpacking,
                    PreferredAccommodation = AccommodationType.Hostel
                }
            },
            new Administrator
            {
                Id = 2,
                Email = "admin@travelshare.com",
                FirstName = "Ana",
                LastName = "Kovaè",
                Department = "User Management",
                ProfileImageUrl = "/images/default-avatar.png",
                CreatedAt = DateTime.UtcNow.AddYears(-1),
                LastLoginAt = DateTime.UtcNow,
                Permissions = new List<string> { "ManageUsers", "ViewReports", "ManageTrips" }
            },
            new Student
            {
                Id = 3,
                Email = "ivan.novak@travelshare.com",
                FirstName = "Ivan",
                LastName = "Novak",
                StudentId = "0246098765",
                University = "Sveuèilište u Splitu",
                Faculty = "Ekonomski fakultet",
                PhoneNumber = "+385 98 765 4321",
                ProfileImageUrl = "/images/default-avatar.png",
                CreatedAt = DateTime.UtcNow.AddMonths(-6),
                Preferences = new TravelPreferences
                {
                    MaxBudget = 800,
                    MinBudget = 200,
                    PreferredDestinations = new List<string> { "Paris", "Rome", "Amsterdam" },
                    PreferredTravelType = TravelType.Cultural,
                    PreferredAccommodation = AccommodationType.Hotel
                }
            }
        };
    }

    public Task<UserBase?> GetUserByIdAsync(int userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        return Task.FromResult(user);
    }

    public Task<UserBase?> GetUserByEmailAsync(string email)
    {
        var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<bool> UpdateUserProfileAsync(UserBase user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            var index = _users.IndexOf(existingUser);
            _users[index] = user;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<IEnumerable<UserBase>> GetAllUsersAsync()
    {
        return Task.FromResult<IEnumerable<UserBase>>(_users);
    }

    public Task<bool> ResetPasswordAsync(string email)
    {
        var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        // In real implementation, this would send a password reset email
        return Task.FromResult(user != null);
    }

    public Task<bool> UpdateTravelPreferencesAsync(int userId, TravelPreferences preferences)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user is Student student)
        {
            student.Preferences = preferences;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
