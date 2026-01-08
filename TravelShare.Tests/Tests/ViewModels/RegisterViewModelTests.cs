using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using TravelShare.Tests.TestHelpers;
using TravelShare.ViewModels;
using Xunit;

namespace TravelShare.Tests.ViewModels;

public class RegisterViewModelTests
{
    [Fact]
    public void RegisterViewModel_WithValidData_PassesValidation()
    {
        // Arrange
        var model = TestDataBuilder.CreateTestRegisterViewModel();

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    public void RegisterViewModel_WithInvalidEmail_FailsValidation(string email)
    {
        // Arrange
        var model = TestDataBuilder.CreateTestRegisterViewModel();
        model.Email = email;

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.Email)));
    }

    [Fact]
    public void RegisterViewModel_WithMismatchedPasswords_FailsValidation()
    {
        // Arrange
        var model = TestDataBuilder.CreateTestRegisterViewModel();
        model.ConfirmPassword = "DifferentPassword";

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.ConfirmPassword)));
    }

    [Fact]
    public void RegisterViewModel_WithoutAcceptingTerms_FailsValidation()
    {
        // Arrange
        var model = TestDataBuilder.CreateTestRegisterViewModel();
        model.AcceptTerms = false;

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains(nameof(RegisterViewModel.AcceptTerms)));
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}