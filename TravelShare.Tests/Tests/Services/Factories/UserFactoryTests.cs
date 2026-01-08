using FluentAssertions;
using TravelShare.Models.Users;
using TravelShare.Services.Factories;
using TravelShare.ViewModels;
using Xunit;

namespace TravelShare.Tests.Services.Factories;

public class UserFactoryTests
{
    private readonly UserFactory _factory;

    public UserFactoryTests()
    {
        _factory = new UserFactory();
    }

    [Fact]
    public void CreateStudent_WithValidModel_ReturnsStudentWithCorrectProperties()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            StudentId = "12345",
            University = "Test University",
            Faculty = "Test Faculty",
            PhoneNumber = "+123456789"
        };

        // Act
        var result = _factory.CreateStudent(model);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Student>();
        result.Email.Should().Be(model.Email);
        result.FirstName.Should().Be(model.FirstName);
        result.LastName.Should().Be(model.LastName);
        result.StudentId.Should().Be(model.StudentId);
        result.University.Should().Be(model.University);
        result.Faculty.Should().Be(model.Faculty);
        result.PhoneNumber.Should().Be(model.PhoneNumber);
        result.IsActive.Should().BeTrue();
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void CreateStudent_Always_CreatesDefaultTravelPreferences()
    {
        // Arrange
        var model = new RegisterViewModel { Email = "test@test.com" };

        // Act
        var result = _factory.CreateStudent(model);

        // Assert
        result.Preferences.Should().NotBeNull();
        result.Preferences.MinBudget.Should().Be(200);
        result.Preferences.MaxBudget.Should().Be(1000);
        result.Preferences.PreferredTravelType.Should().Be(TravelType.Beach);
        result.Preferences.PreferredAccommodation.Should().Be(AccommodationType.Hostel);
        result.Preferences.PreferredDestinations.Should().BeEquivalentTo(new[] { "Hrvatska", "Italija" });
    }

    [Fact]
    public void CreateStudent_Always_GeneratesUniqueId()
    {
        // Arrange
        var model = new RegisterViewModel { Email = "test@test.com" };

        // Act
        var student1 = _factory.CreateStudent(model);
        var student2 = _factory.CreateStudent(model);

        // Assert
        student1.Id.Should().NotBe(student2.Id);
        student1.Id.Should().BeInRange(1000, 9999);
        student2.Id.Should().BeInRange(1000, 9999);
    }

    [Fact]
    public void CreateAdministrator_WithValidParams_ReturnsAdminWithCorrectProperties()
    {
        // Arrange
        const string email = "admin@test.com";
        const string firstName = "Admin";
        const string lastName = "User";

        // Act
        var result = _factory.CreateAdministrator(email, firstName, lastName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Administrator>();
        result.Email.Should().Be(email);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
        result.Department.Should().BeEmpty();
        result.IsActive.Should().BeTrue();
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.Id.Should().BeInRange(1000, 9999);
    }
}