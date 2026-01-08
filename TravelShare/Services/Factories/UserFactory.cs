using TravelShare.ViewModels;
using TravelShare.Models.Users;

namespace TravelShare.Services.Factories;
public class UserFactory : IUserFactory
{
    public Student CreateStudent(RegisterViewModel model)
    {
        return new Student
        {
            Id = new Random().Next(1000, 9999),
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            StudentId = model.StudentId,
            University = model.University,
            Faculty = model.Faculty,
            PhoneNumber = model.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Preferences = new TravelPreferences
            {
                MinBudget = 200,
                MaxBudget = 1000,
                PreferredTravelType = TravelType.Beach,
                PreferredAccommodation = AccommodationType.Hostel,
                PreferredDestinations = new List<string> { "Hrvatska", "Italija" }
            }
        };
    }

    public Administrator CreateAdministrator(string email, string firstName, string lastName)
    {
        return new Administrator
        {
            Id = new Random().Next(1000, 9999),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Department = string.Empty,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }
}