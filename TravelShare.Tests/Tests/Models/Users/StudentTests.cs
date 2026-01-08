using FluentAssertions;
using TravelShare.Models.Users;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Models.Users;

public class StudentTests
{
    [Fact]
    public void GetUserType_ReturnsStudent()
    {
        // Arrange
        var student = TestDataBuilder.CreateTestStudent();

        // Act
        var userType = student.GetUserType();

        // Assert
        userType.Should().Be("Student");
    }

    [Fact]
    public void GetDisplayName_ReturnsFullNameWithStudentId()
    {
        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Doe",
            StudentId = "12345"
        };

        // Act
        var displayName = student.GetDisplayName();

        // Assert
        displayName.Should().Be("John Doe (12345)");
    }

    [Fact]
    public void GetUserDescription_ReturnsStudentDescription()
    {
        // Arrange
        var student = new Student
        {
            FirstName = "John",
            LastName = "Doe",
            University = "Test University",
            Faculty = "Computer Science"
        };

        // Act
        var description = student.GetUserDescription();

        // Assert
        description.Should().Contain("Student");
        description.Should().Contain("Test University");
        description.Should().Contain("Computer Science");
    }

    [Fact]
    public void Student_WithPreferences_StoresPreferencesCorrectly()
    {
        // Arrange
        var preferences = new TravelPreferences
        {
            MinBudget = 100,
            MaxBudget = 500,
            PreferredTravelType = TravelType.Adventure,
            PreferredAccommodation = AccommodationType.Hostel,
            PreferredDestinations = new List<string> { "Spain", "Italy" }
        };

        var student = new Student
        {
            Preferences = preferences
        };

        // Act & Assert
        student.Preferences.Should().Be(preferences);
        student.Preferences!.MinBudget.Should().Be(100);
        student.Preferences.MaxBudget.Should().Be(500);
        student.Preferences.PreferredDestinations.Should().BeEquivalentTo(new[] { "Spain", "Italy" });
    }

    [Theory]
    [InlineData("", "", "", "")]
    [InlineData("John", "", "12345", "John (12345)")]
    [InlineData("", "Doe", "12345", "Doe (12345)")]
    [InlineData("John", "Doe", "12345", "John Doe (12345)")]
    [InlineData("John", "Doe", "", "John Doe")]
    [InlineData("", "", "12345", "(12345)")]
    public void GetDisplayName_WithVariousNames_ReturnsCorrectFormat(string firstName, string lastName, string studentId, string expected)
    {
        // Arrange
        var student = new Student
        {
            FirstName = firstName,
            LastName = lastName,
            StudentId = studentId
        };

        // Act
        var result = student.GetDisplayName();

        // Assert
        result.Should().Be(expected);
    }
}