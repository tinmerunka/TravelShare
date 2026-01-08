using FluentAssertions;
using Moq;
using TravelShare.Models.Users;
using TravelShare.Services;
using TravelShare.Services.Interfaces;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Services;

public class MockUserServiceTests
{
    private readonly MockUserService _userService;
    private readonly Mock<IProfileUpdateObserver> _mockObserver;

    public MockUserServiceTests()
    {
        _userService = new MockUserService();
        _mockObserver = new Mock<IProfileUpdateObserver>();
    }

    #region GetUserByIdAsync Tests

    [Fact]
    public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        const int userId = 1;

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Should().BeOfType<Student>();
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        const int invalidUserId = 999;

        // Act
        var result = await _userService.GetUserByIdAsync(invalidUserId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(1, typeof(Student))]
    [InlineData(2, typeof(Administrator))]
    [InlineData(3, typeof(Student))]
    public async Task GetUserByIdAsync_WithDifferentIds_ReturnsCorrectUserTypes(int userId, Type expectedType)
    {
        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedType);
        result!.Id.Should().Be(userId);
    }

    #endregion

    #region GetUserByEmailAsync Tests

    [Fact]
    public async Task GetUserByEmailAsync_WithValidEmail_ReturnsUser()
    {
        // Arrange
        const string email = "student@travelshare.com";

        // Act
        var result = await _userService.GetUserByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("STUDENT@TRAVELSHARE.COM")]
    [InlineData("Student@TravelShare.Com")]
    public async Task GetUserByEmailAsync_WithDifferentCasing_ReturnsUserIgnoreCase(string email)
    {
        // Act
        var result = await _userService.GetUserByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("student@travelshare.com");
    }

    [Fact]
    public async Task GetUserByEmailAsync_WithNonExistentEmail_ReturnsNull()
    {
        // Act
        var result = await _userService.GetUserByEmailAsync("nonexistent@test.com");

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetAllUsersAsync Tests

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        var usersList = result.ToList();
        usersList[0].Should().BeOfType<Student>();
        usersList[1].Should().BeOfType<Administrator>();
        usersList[2].Should().BeOfType<Student>();
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsUsersWithCorrectEmails()
    {
        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        var emails = result.Select(u => u.Email).ToList();
        emails.Should().BeEquivalentTo(new[]
        {
            "student@travelshare.com",
            "admin@travelshare.com",
            "ivan.novak@travelshare.com"
        });
    }

    #endregion

    #region UpdateUserProfileAsync Tests

    [Fact]
    public async Task UpdateUserProfileAsync_WithExistingUser_ReturnsTrue()
    {
        // Arrange
        var existingUser = await _userService.GetUserByIdAsync(1);
        existingUser!.FirstName = "UpdatedName";

        // Act
        var result = await _userService.UpdateUserProfileAsync(existingUser);

        // Assert
        result.Should().BeTrue();

        var updatedUser = await _userService.GetUserByIdAsync(1);
        updatedUser!.FirstName.Should().Be("UpdatedName");
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WithNonExistingUser_ReturnsFalse()
    {
        // Arrange
        var newUser = TestDataBuilder.CreateTestStudent();
        newUser.Id = 999;

        // Act
        var result = await _userService.UpdateUserProfileAsync(newUser);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WithObserver_NotifiesObserver()
    {
        // Arrange
        _userService.Subscribe(_mockObserver.Object);
        var existingUser = await _userService.GetUserByIdAsync(1);

        // Act
        await _userService.UpdateUserProfileAsync(existingUser!);

        // Assert
        _mockObserver.Verify(o => o.OnProfileUpdatedAsync(existingUser), Times.Once);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_WithMultipleObservers_NotifiesAll()
    {
        // Arrange
        var mockObserver2 = new Mock<IProfileUpdateObserver>();
        _userService.Subscribe(_mockObserver.Object);
        _userService.Subscribe(mockObserver2.Object);

        var existingUser = await _userService.GetUserByIdAsync(1);

        // Act
        await _userService.UpdateUserProfileAsync(existingUser!);

        // Assert
        _mockObserver.Verify(o => o.OnProfileUpdatedAsync(existingUser), Times.Once);
        mockObserver2.Verify(o => o.OnProfileUpdatedAsync(existingUser), Times.Once);
    }

    #endregion

    #region ResetPasswordAsync Tests

    [Fact]
    public async Task ResetPasswordAsync_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        const string email = "student@travelshare.com";

        // Act
        var result = await _userService.ResetPasswordAsync(email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ResetPasswordAsync_WithNonExistingEmail_ReturnsFalse()
    {
        // Act
        var result = await _userService.ResetPasswordAsync("nonexistent@test.com");

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("STUDENT@TRAVELSHARE.COM")]
    [InlineData("Student@TravelShare.Com")]
    public async Task ResetPasswordAsync_WithDifferentCasing_ReturnsTrue(string email)
    {
        // Act
        var result = await _userService.ResetPasswordAsync(email);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region UpdateTravelPreferencesAsync Tests

    [Fact]
    public async Task UpdateTravelPreferencesAsync_WithStudentUser_ReturnsTrue()
    {
        // Arrange
        const int studentId = 1;
        var newPreferences = TestDataBuilder.CreateTestTravelPreferences();

        // Act
        var result = await _userService.UpdateTravelPreferencesAsync(studentId, newPreferences);

        // Assert
        result.Should().BeTrue();

        var student = await _userService.GetUserByIdAsync(studentId) as Student;
        student!.Preferences!.MaxBudget.Should().Be(newPreferences.MaxBudget);
        student.Preferences.MinBudget.Should().Be(newPreferences.MinBudget);
    }

    [Fact]
    public async Task UpdateTravelPreferencesAsync_WithAdministratorUser_ReturnsFalse()
    {
        // Arrange
        const int adminId = 2;
        var newPreferences = TestDataBuilder.CreateTestTravelPreferences();

        // Act
        var result = await _userService.UpdateTravelPreferencesAsync(adminId, newPreferences);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateTravelPreferencesAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        const int nonExistentId = 999;
        var newPreferences = TestDataBuilder.CreateTestTravelPreferences();

        // Act
        var result = await _userService.UpdateTravelPreferencesAsync(nonExistentId, newPreferences);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Observer Pattern Tests

    [Fact]
    public void Subscribe_WithValidObserver_DoesNotThrow()
    {
        // Act & Assert
        var action = () => _userService.Subscribe(_mockObserver.Object);
        action.Should().NotThrow();
    }

    [Fact]
    public void Subscribe_WithNullObserver_DoesNotThrow()
    {
        // Act & Assert
        var action = () => _userService.Subscribe(null!);
        action.Should().NotThrow();
    }

    [Fact]
    public void Unsubscribe_WithExistingObserver_DoesNotThrow()
    {
        // Arrange
        _userService.Subscribe(_mockObserver.Object);

        // Act & Assert
        var action = () => _userService.Unsubscribe(_mockObserver.Object);
        action.Should().NotThrow();
    }

    [Fact]
    public void Unsubscribe_WithNullObserver_DoesNotThrow()
    {
        // Act & Assert
        var action = () => _userService.Unsubscribe(null!);
        action.Should().NotThrow();
    }

    [Fact]
    public async Task Subscribe_TwiceWithSameObserver_OnlyNotifiesOnce()
    {
        // Arrange
        _userService.Subscribe(_mockObserver.Object);
        _userService.Subscribe(_mockObserver.Object); // Subscribe twice

        var existingUser = await _userService.GetUserByIdAsync(1);

        // Act
        await _userService.UpdateUserProfileAsync(existingUser!);

        // Assert
        _mockObserver.Verify(o => o.OnProfileUpdatedAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Unsubscribe_AfterSubscribe_DoesNotNotify()
    {
        // Arrange
        _userService.Subscribe(_mockObserver.Object);
        _userService.Unsubscribe(_mockObserver.Object);

        var existingUser = await _userService.GetUserByIdAsync(1);

        // Act
        await _userService.UpdateUserProfileAsync(existingUser!);

        // Assert
        _mockObserver.Verify(o => o.OnProfileUpdatedAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task MockUserService_InitialData_HasCorrectStudentPreferences()
    {
        // Act
        var student1 = await _userService.GetUserByIdAsync(1) as Student;
        var student3 = await _userService.GetUserByIdAsync(3) as Student;

        // Assert
        student1!.Preferences.Should().NotBeNull();
        student1.Preferences!.PreferredTravelType.Should().Be(TravelType.Backpacking);
        student1.Preferences.PreferredAccommodation.Should().Be(AccommodationType.Hostel);

        student3!.Preferences.Should().NotBeNull();
        student3.Preferences!.PreferredTravelType.Should().Be(TravelType.Cultural);
        student3.Preferences.PreferredAccommodation.Should().Be(AccommodationType.Hotel);
    }

    [Fact]
    public async Task MockUserService_InitialData_HasCorrectAdministratorPermissions()
    {
        // Act
        var admin = await _userService.GetUserByIdAsync(2) as Administrator;

        // Assert
        admin.Should().NotBeNull();
        admin!.Department.Should().Be("User Management");
        admin.Permissions.Should().BeEquivalentTo(new[] { "ManageUsers", "ViewReports", "ManageTrips" });
    }

    #endregion
}