// TravelShare.Tests/ViewModels/LoginViewModelTests.cs
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using TravelShare.ViewModels;
using Xunit;

namespace TravelShare.Tests.ViewModels;

public class LoginViewModelTests
{
    [Fact]
    public void LoginViewModel_WithValidData_PassesValidation()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = "test@example.com",
            Password = "ValidPassword123"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    public void LoginViewModel_WithInvalidEmail_FailsValidation(string email)
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = email,
            Password = "ValidPassword123"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains(nameof(LoginViewModel.Email)));
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}