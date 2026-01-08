// TravelShare.Tests/TestHelpers/TestDataBuilder.cs (Updated)
using TravelShare.Models.Authentication;
using TravelShare.Models.Users;
using TravelShare.ViewModels;

namespace TravelShare.Tests.TestHelpers;

public static class TestDataBuilder
{
    public static Student CreateTestStudent(int id = 1)
    {
        return new Student
        {
            Id = id,
            Email = $"student{id}@test.com",
            FirstName = "Test",
            LastName = "Student",
            StudentId = "12345",
            University = "Test University",
            Faculty = "Test Faculty",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Preferences = new TravelPreferences
            {
                MinBudget = 200,
                MaxBudget = 1000,
                PreferredTravelType = TravelType.Beach,
                PreferredAccommodation = AccommodationType.Hostel,
                PreferredDestinations = new List<string> { "Croatia", "Italy" }
            }
        };
    }

    public static Administrator CreateTestAdministrator(int id = 1)
    {
        return new Administrator
        {
            Id = id,
            Email = $"admin{id}@test.com",
            FirstName = "Test",
            LastName = "Admin",
            Department = "IT",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Permissions = new List<string> { "ManageUsers", "ViewReports" }
        };
    }

    public static LoginViewModel CreateTestLoginViewModel()
    {
        return new LoginViewModel
        {
            Email = "test@test.com",
            Password = "TestPassword123"
        };
    }

    public static RegisterViewModel CreateTestRegisterViewModel()
    {
        return new RegisterViewModel
        {
            Email = "new@test.com",
            FirstName = "New",
            LastName = "User",
            StudentId = "54321",
            University = "New University",
            Faculty = "New Faculty",
            PhoneNumber = "+987654321",
            Password = "TestPassword123",
            ConfirmPassword = "TestPassword123",
            AcceptTerms = true // This is crucial for validation to pass
        };
    }

    public static ChangePasswordViewModel CreateTestChangePasswordViewModel()
    {
        return new ChangePasswordViewModel
        {
            CurrentPassword = "OldPassword123",
            NewPassword = "NewPassword123",
            ConfirmPassword = "NewPassword123"
        };
    }

    public static UpdateProfileViewModel CreateTestUpdateProfileViewModel()
    {
        return new UpdateProfileViewModel
        {
            FirstName = "Updated",
            LastName = "User",
            Email = "updated@test.com",
            StudentId = "67890",
            University = "Updated University",
            Faculty = "Updated Faculty",
            PhoneNumber = "+1234567890",
            MinBudget = 300,
            MaxBudget = 1500,
            PreferredTravelType = TravelType.Cultural.ToString(),
            PreferredAccommodation = AccommodationType.Hotel.ToString(),
            PreferredDestinations = "Paris, London, Berlin"
        };
    }

    public static UserProfileViewModel CreateTestUserProfileViewModel(User user)
    {
        return new UserProfileViewModel
        {
            User = user,
            StudentProfile = user as Student,
            AdminProfile = user as Administrator
        };
    }

    public static TravelPreferences CreateTestTravelPreferences()
    {
        return new TravelPreferences
        {
            MinBudget = 200,
            MaxBudget = 1000,
            PreferredTravelType = TravelType.Adventure,
            PreferredAccommodation = AccommodationType.Hostel,
            PreferredDestinations = new List<string> { "Spain", "Portugal", "France" }
        };
    }

    public static AuthenticationResult CreateSuccessAuthResult(User user)
    {
        return AuthenticationResult.Success(user);
    }

    public static AuthenticationResult CreateFailureAuthResult(string error = "Authentication failed")
    {
        return AuthenticationResult.Failed(error);
    }

}