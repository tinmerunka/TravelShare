// TravelShare.Tests/Controllers/AccountControllerTests.cs
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using TravelShare.Controllers;
using TravelShare.Models.Authentication;
using TravelShare.Models.Users;
using TravelShare.Services.Factories;
using TravelShare.Services.Interfaces;
using TravelShare.ViewModels;
using Xunit;

namespace TravelShare.Tests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IUserFactory> _mockUserFactory;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<ILogger<AccountController>> _mockLogger;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();
        _mockUserService = new Mock<IUserService>();
        _mockUserFactory = new Mock<IUserFactory>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockLogger = new Mock<ILogger<AccountController>>();

        _controller = new AccountController(
            _mockAuthService.Object,
            _mockUserService.Object,
            _mockUserFactory.Object,
            _mockCurrentUserService.Object,
            _mockLogger.Object);

        // Setup TempData properly
        _controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>());
    }

    #region Login Tests

    [Fact]
    public void Login_Get_WhenUserNotLoggedIn_ReturnsView()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.IsUserLoggedIn()).Returns(false);

        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Login_Get_WhenUserLoggedIn_RedirectsToHome()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.IsUserLoggedIn()).Returns(true);

        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");
    }

    [Fact]
    public async Task Login_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var model = new LoginViewModel { Email = "", Password = "" };
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _controller.Login(model);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.Model.Should().Be(model);
    }

    [Fact]
    public async Task Login_Post_WithValidCredentials_RedirectsToHome()
    {
        // Arrange
        var model = new LoginViewModel { Email = "test@test.com", Password = "password" };
        var user = CreateTestStudent();
        var authResult = AuthenticationResult.Success(user);

        _mockAuthService.Setup(x => x.AuthenticateAsync(model.Email, model.Password))
                       .ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");

        _mockCurrentUserService.Verify(x => x.StoreCurrentUser(user), Times.Once);
    }

    [Fact]
    public async Task Login_Post_WithInvalidCredentials_ReturnsViewWithError()
    {
        // Arrange
        var model = new LoginViewModel { Email = "test@test.com", Password = "wrong" };
        var authResult = AuthenticationResult.Failed("Invalid credentials");

        _mockAuthService.Setup(x => x.AuthenticateAsync(model.Email, model.Password))
                       .ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(model);

        // Assert
        result.Should().BeOfType<ViewResult>();
        _controller.ModelState.Should().ContainKey(string.Empty);
    }

    #endregion

    #region Register Tests

    [Fact]
    public void Register_Get_WhenUserNotLoggedIn_ReturnsView()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.IsUserLoggedIn()).Returns(false);

        // Act
        var result = _controller.Register();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Register_Get_WhenUserLoggedIn_RedirectsToHome()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.IsUserLoggedIn()).Returns(true);

        // Act
        var result = _controller.Register();

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task Register_Post_WithExistingEmail_ReturnsViewWithError()
    {
        // Arrange
        var model = CreateTestRegisterViewModel();
        var existingUser = CreateTestStudent();

        _mockUserService.Setup(x => x.GetUserByEmailAsync(model.Email))
                       .ReturnsAsync(existingUser);

        // Act
        var result = await _controller.Register(model);

        // Assert
        result.Should().BeOfType<ViewResult>();
        _controller.ModelState.Should().ContainKey("Email");
    }

    [Fact]
    public async Task Register_Post_WithValidModel_RedirectsToHomeWithSuccess()
    {
        // Arrange
        var model = CreateTestRegisterViewModel();
        var newStudent = CreateTestStudent();

        _mockUserService.Setup(x => x.GetUserByEmailAsync(model.Email))
                       .ReturnsAsync((User?)null);
        _mockUserFactory.Setup(x => x.CreateStudent(model))
                       .Returns(newStudent);

        // Act
        var result = await _controller.Register(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _mockCurrentUserService.Verify(x => x.StoreCurrentUser(newStudent), Times.Once);
        _controller.TempData["SuccessMessage"].Should().NotBeNull();
    }

    #endregion

    #region Profile Tests

    [Fact]
    public void Profile_WhenUserNotLoggedIn_RedirectsToLogin()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns((User?)null);

        // Act
        var result = _controller.Profile();

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Login");
    }

    [Fact]
    public void Profile_WhenUserLoggedIn_ReturnsViewWithModel()
    {
        // Arrange
        var user = CreateTestStudent();
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns(user);

        // Act
        var result = _controller.Profile();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.Model.Should().BeOfType<UserProfileViewModel>();
        var model = viewResult.Model as UserProfileViewModel;
        model!.User.Should().Be(user);
        model.StudentProfile.Should().Be(user);
    }

    #endregion

    #region Update Profile Tests

    [Fact]
    public async Task UpdateProfile_WhenUserNotLoggedIn_RedirectsToLogin()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns((User?)null);

        // Act
        var result = await _controller.UpdateProfile(new UpdateProfileViewModel());

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Login");
    }

    [Fact]
    public async Task UpdateProfile_WithInvalidModel_RedirectsToProfile()
    {
        // Arrange
        _controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _controller.UpdateProfile(new UpdateProfileViewModel());

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Profile");
    }

    [Fact]
    public async Task UpdateProfile_WithValidStudent_UpdatesAndRedirects()
    {
        // Arrange
        var student = CreateTestStudentWithPreferences();
        var model = new UpdateProfileViewModel
        {
            FirstName = "NewName",
            Email = "new@test.com",
            MinBudget = 500,
            MaxBudget = 1500
        };

        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns(student);
        _mockUserService.Setup(x => x.UpdateUserProfileAsync(It.IsAny<User>()))
                       .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateProfile(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        student.FirstName.Should().Be("NewName");
        student.Email.Should().Be("new@test.com");
        student.Preferences!.MinBudget.Should().Be(500);
        _mockCurrentUserService.Verify(x => x.StoreCurrentUser(student), Times.Once);
        _controller.TempData["SuccessMessage"].Should().NotBeNull();
    }

    #endregion

    #region Logout Tests

    [Fact]
    public void Logout_CallsServicesAndRedirectsToHome()
    {
        // Arrange
        var user = CreateTestStudent();
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns(user);

        // Act
        var result = _controller.Logout();

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");

        _mockAuthService.Verify(x => x.Logout(), Times.Once);
        _mockCurrentUserService.Verify(x => x.ClearCurrentUser(), Times.Once);
    }

    #endregion

    #region Change Password Tests

    [Fact]
    public async Task ChangePassword_WithInvalidModel_RedirectsWithError()
    {
        // Arrange
        _controller.ModelState.AddModelError("NewPassword", "Required");

        // Act
        var result = await _controller.ChangePassword(new ChangePasswordViewModel());

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _controller.TempData["ErrorMessage"].Should().NotBeNull();
    }

    [Fact]
    public async Task ChangePassword_WithMismatchedPasswords_RedirectsWithError()
    {
        // Arrange
        var model = new ChangePasswordViewModel
        {
            CurrentPassword = "oldpass",
            NewPassword = "password1",
            ConfirmPassword = "password2"
        };

        // Act
        var result = await _controller.ChangePassword(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var errorMessage = _controller.TempData["ErrorMessage"]?.ToString();
        errorMessage.Should().NotBeNull();
        errorMessage.Should().Contain("don't match");
    }

    [Fact]
    public async Task ChangePassword_WithValidModel_RedirectsWithSuccess()
    {
        // Arrange
        var model = new ChangePasswordViewModel
        {
            CurrentPassword = "oldpass",
            NewPassword = "newpass",
            ConfirmPassword = "newpass"
        };
        var user = CreateTestStudent();

        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns(user);

        // Act
        var result = await _controller.ChangePassword(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _controller.TempData["SuccessMessage"].Should().NotBeNull();
    }

    #endregion

    #region Helper Methods

    private static Student CreateTestStudent()
    {
        return new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            StudentId = "123",
            University = "Test University",
            Faculty = "Test Faculty",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static Student CreateTestStudentWithPreferences()
    {
        var student = CreateTestStudent();
        student.Preferences = new TravelPreferences
        {
            MinBudget = 200,
            MaxBudget = 1000,
            PreferredTravelType = TravelType.Beach,
            PreferredAccommodation = AccommodationType.Hostel,
            PreferredDestinations = new List<string> { "Croatia", "Italy" }
        };
        return student;
    }

    private static RegisterViewModel CreateTestRegisterViewModel()
    {
        return new RegisterViewModel
        {
            Email = "new@test.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "TestPassword123",
            ConfirmPassword = "TestPassword123",
            StudentId = "123",
            University = "Test University",
            Faculty = "Test Faculty",
            AcceptTerms = true
        };
    }

    #endregion
}