using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using TravelShare.Tests.TestHelpers;
using TravelShare.ViewModels;
using Xunit;

namespace TravelShare.Tests.ViewModels;

public class ChangePasswordViewModelTests
{
    [Fact]
    public void ChangePasswordViewModel_WithValidData_PassesValidation()
    {
        // Arrange
        var model = TestDataBuilder.CreateTestChangePasswordViewModel();

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("12345")] // Too short
    public void ChangePasswordViewModel_WithInvalidNewPassword_FailsValidation(string newPassword)
    {
        // Arrange
        var model = TestDataBuilder.CreateTestChangePasswordViewModel();
        model.NewPassword = newPassword;

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains(nameof(ChangePasswordViewModel.NewPassword)));
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}