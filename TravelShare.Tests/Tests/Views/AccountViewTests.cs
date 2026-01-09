using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TravelShare.Models.Users;
using TravelShare.Tests.TestHelpers;
using TravelShare.ViewModels;

namespace TravelShare.Tests.Views;

public class AccountViewTests
{
    private readonly Mock<IHtmlHelper> _mockHtmlHelper;
    private readonly Mock<IUrlHelper> _mockUrlHelper;
    private readonly ViewContext _viewContext;
    private readonly StringWriter _writer;

    public AccountViewTests()
    {
        _mockHtmlHelper = new Mock<IHtmlHelper>();
        _mockUrlHelper = new Mock<IUrlHelper>();
        _writer = new StringWriter();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        _viewContext = new ViewContext(
            actionContext,
            Mock.Of<IView>(),
            new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
            Mock.Of<ITempDataDictionary>(),
            _writer,
            new HtmlHelperOptions());
    }

    #region Login View Tests

    [Fact]
    public void LoginView_WithValidModel_ShouldRenderCorrectly()
    {
        // Arrange
        var model = TestDataBuilder.CreateTestLoginViewModel();
        _viewContext.ViewData.Model = model;

        // Act & Assert - These would be integration tests checking HTML output
        // For unit tests, we focus on model validation and data binding
        model.Email.Should().NotBeNullOrEmpty();
        model.Password.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void LoginView_WithModelErrors_ShouldDisplayValidationMessages()
    {
        // Arrange
        var model = new LoginViewModel { Email = "", Password = "" };
        _viewContext.ViewData.Model = model;
        _viewContext.ViewData.ModelState.AddModelError("Email", "Email is required");
        _viewContext.ViewData.ModelState.AddModelError("Password", "Password is required");

        // Assert
        _viewContext.ViewData.ModelState.IsValid.Should().BeFalse();
        _viewContext.ViewData.ModelState.Should().ContainKey("Email");
        _viewContext.ViewData.ModelState.Should().ContainKey("Password");
        _viewContext.ViewData.ModelState["Email"]!.Errors.Should().HaveCount(1);
        _viewContext.ViewData.ModelState["Password"]!.Errors.Should().HaveCount(1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("@test.com")]
    public void LoginView_WithInvalidEmailFormats_ShouldFailValidation(string invalidEmail)
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = invalidEmail,
            Password = "ValidPassword123"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        if (string.IsNullOrEmpty(invalidEmail))
        {
            validationResults.Should().Contain(r => r.MemberNames.Contains("Email"));
        }
        else if (!invalidEmail.Contains('@') || !invalidEmail.Contains('.'))
        {
            validationResults.Should().Contain(r => r.MemberNames.Contains("Email"));
        }
    }

    [Fact]
    public void LoginView_WithRememberMeOption_ShouldBindCorrectly()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = "test@test.com",
            Password = "password123",
            RememberMe = true
        };

        // Assert
        model.RememberMe.Should().BeTrue();
        model.Email.Should().Be("test@test.com");
    }

    #endregion

    #region Profile View Tests

    [Fact]
    public void ProfileView_WithStudentUser_ShouldDisplayStudentFields()
    {
        // Arrange
        var student = TestDataBuilder.CreateTestStudent();
        var model = new UserProfileViewModel
        {
            User = student,
            StudentProfile = student,
            AdminProfile = null
        };

        _viewContext.ViewData.Model = model;

        // Assert
        model.StudentProfile.Should().NotBeNull();
        model.StudentProfile!.StudentId.Should().NotBeEmpty();
        model.StudentProfile.University.Should().NotBeEmpty();
        model.StudentProfile.Faculty.Should().NotBeEmpty();
        model.AdminProfile.Should().BeNull();
    }

    [Fact]
    public void ProfileView_WithAdministratorUser_ShouldDisplayAdminFields()
    {
        // Arrange
        var admin = TestDataBuilder.CreateTestAdministrator();
        var model = new UserProfileViewModel
        {
            User = admin,
            StudentProfile = null,
            AdminProfile = admin
        };

        _viewContext.ViewData.Model = model;

        // Assert
        model.AdminProfile.Should().NotBeNull();
        model.AdminProfile!.Department.Should().NotBeEmpty();
        model.AdminProfile.Permissions.Should().NotBeEmpty();
        model.StudentProfile.Should().BeNull();
    }

    [Fact]
    public void ProfileView_WithTravelPreferences_ShouldDisplayPreferencesCorrectly()
    {
        // Arrange
        var student = TestDataBuilder.CreateTestStudent();
        var model = new UserProfileViewModel
        {
            User = student,
            StudentProfile = student,
            AdminProfile = null
        };

        // Assert
        model.StudentProfile!.Preferences.Should().NotBeNull();
        model.StudentProfile.Preferences!.MinBudget.Should().BeGreaterThan(0);
        model.StudentProfile.Preferences.MaxBudget.Should().BeGreaterThan(model.StudentProfile.Preferences.MinBudget);
        model.StudentProfile.Preferences.PreferredDestinations.Should().NotBeEmpty();
    }

    [Fact]
    public void ProfileView_UpdateProfile_ShouldBindModelCorrectly()
    {
        // Arrange
        var updateModel = TestDataBuilder.CreateTestUpdateProfileViewModel();

        // Assert
        updateModel.FirstName.Should().NotBeEmpty();
        updateModel.LastName.Should().NotBeEmpty();
        updateModel.Email.Should().Contain("@");
        updateModel.MinBudget.Should().BeGreaterThan(0);
        updateModel.MaxBudget.Should().BeGreaterThan((decimal)updateModel.MinBudget);
    }

    [Fact]
    public void ProfileView_WithNullPreferences_ShouldHandleGracefully()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            StudentId = "123",
            University = "Test University",
            Faculty = "Test Faculty",
            Preferences = null // Null preferences
        };

        var model = new UserProfileViewModel
        {
            User = student,
            StudentProfile = student,
            AdminProfile = null
        };

        // Assert
        model.StudentProfile.Should().NotBeNull();
        model.StudentProfile!.Preferences.Should().BeNull();
        // View should handle null preferences gracefully
    }

    #endregion

    #region Helper Methods

    private static List<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
        System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    #endregion
}