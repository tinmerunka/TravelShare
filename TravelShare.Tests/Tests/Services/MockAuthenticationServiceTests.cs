using FluentAssertions;
using TravelShare.Services;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Services;

public class MockAuthenticationServiceTests
{
    private readonly MockAuthenticationService _authService;

    public MockAuthenticationServiceTests()
    {
        _authService = new MockAuthenticationService();
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsSuccessResult()
    {
        // Arrange
        const string email = "student@travelshare.com";
        const string password = "password"; // Changed to match EmailAuthenticationProvider

        // Act
        var result = await _authService.AuthenticateAsync(email, password);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.User.Should().NotBeNull();
        result.User!.Email.Should().Be(email);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidEmail_ReturnsFailureResult()
    {
        // Arrange
        const string invalidEmail = "nonexistent@test.com";
        const string password = "password";

        // Act
        var result = await _authService.AuthenticateAsync(invalidEmail, password);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.User.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ReturnsFailureResult()
    {
        // Arrange
        const string email = "student@travelshare.com";
        const string invalidPassword = "wrongpassword";

        // Act
        var result = await _authService.AuthenticateAsync(email, invalidPassword);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.User.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AuthenticateAsync_WithSuccessfulLogin_SetsCurrentUser()
    {
        // Arrange
        const string email = "student@travelshare.com";
        const string password = "password"; // Changed to match EmailAuthenticationProvider

        // Act
        var result = await _authService.AuthenticateAsync(email, password);

        // Assert
        _authService.GetCurrentUser().Should().NotBeNull();
        _authService.GetCurrentUser()!.Email.Should().Be(email);
    }

    [Fact]
    public async Task RegisterUserAsync_WithValidStudent_ReturnsTrue()
    {
        // Arrange
        var student = TestDataBuilder.CreateTestStudent();
        const string password = "password123";

        // Act
        var result = await _authService.RegisterUserAsync(student, password);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void SetCurrentUser_WithValidUser_SetsUser()
    {
        // Arrange
        var user = TestDataBuilder.CreateTestStudent();

        // Act
        _authService.SetCurrentUser(user);

        // Assert
        _authService.GetCurrentUser().Should().Be(user);
    }

    [Fact]
    public void GetCurrentUser_WithNoUserSet_ReturnsNull()
    {
        // Act
        var currentUser = _authService.GetCurrentUser();

        // Assert
        currentUser.Should().BeNull();
    }

    [Fact]
    public void Logout_ClearsCurrentUser()
    {
        // Arrange
        var user = TestDataBuilder.CreateTestStudent();
        _authService.SetCurrentUser(user);

        // Act
        _authService.Logout();

        // Assert
        _authService.GetCurrentUser().Should().BeNull();
    }

    [Theory]
    [InlineData("student@travelshare.com", "password")]
    [InlineData("admin@travelshare.com", "adminpass")]
    [InlineData("ivan.novak@travelshare.com", "ivanpass")]
    public async Task AuthenticateAsync_WithKnownUsers_ReturnsSuccess(string email, string password)
    {
        // Act
        var result = await _authService.AuthenticateAsync(email, password);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.User!.Email.Should().Be(email);
    }
}