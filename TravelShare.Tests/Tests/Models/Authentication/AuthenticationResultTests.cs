using FluentAssertions;
using TravelShare.Models.Authentication;
using TravelShare.Models.Users;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Models.Authentication;

public class AuthenticationResultTests
{
    [Fact]
    public void Success_WithValidUser_ReturnsSuccessResult()
    {
        // Arrange
        var user = TestDataBuilder.CreateTestStudent();

        // Act
        var result = AuthenticationResult.Success(user);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.User.Should().Be(user);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Failed_WithErrorMessage_ReturnsFailureResult()
    {
        // Arrange
        const string errorMessage = "Invalid credentials";

        // Act
        var result = AuthenticationResult.Failed(errorMessage);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.User.Should().BeNull();
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Authentication failed")]
    [InlineData("User not found")]
    public void Failed_WithVariousErrorMessages_SetsErrorMessageCorrectly(string errorMessage)
    {
        // Act
        var result = AuthenticationResult.Failed(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void Success_WithNullUser_ThrowsArgumentNullException()
    {
        // Act & Assert
        var action = () => AuthenticationResult.Success(null!);
        action.Should().Throw<ArgumentNullException>();
    }
}