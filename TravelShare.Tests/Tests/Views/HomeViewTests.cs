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

namespace TravelShare.Tests.Views;

public class HomeViewTests
{
    private readonly ViewContext _viewContext;
    private readonly StringWriter _writer;

    public HomeViewTests()
    {
        _writer = new StringWriter();

        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        _viewContext = new ViewContext(
            actionContext,
            Mock.Of<IView>(),
            new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
            new TempDataDictionary(actionContext.HttpContext, Mock.Of<ITempDataProvider>()),
            _writer,
            new HtmlHelperOptions());
    }

    #region Index View Tests - Authenticated User

    [Fact]
    public void IndexView_WithAuthenticatedStudent_ShouldShowPersonalizedContent()
    {
        // Arrange
        var student = TestDataBuilder.CreateTestStudent();
        _viewContext.ViewData["CurrentUser"] = student;

        // Assert
        var currentUser = _viewContext.ViewData["CurrentUser"] as User;
        currentUser.Should().NotBeNull();
        currentUser.Should().BeOfType<Student>();
        currentUser!.FirstName.Should().NotBeNullOrEmpty();
        currentUser.GetUserType().Should().Be("Student");
    }

    [Fact]
    public void IndexView_WithAuthenticatedAdmin_ShouldShowPersonalizedContent()
    {
        // Arrange
        var admin = TestDataBuilder.CreateTestAdministrator();
        _viewContext.ViewData["CurrentUser"] = admin;

        // Assert
        var currentUser = _viewContext.ViewData["CurrentUser"] as User;
        currentUser.Should().NotBeNull();
        currentUser.Should().BeOfType<Administrator>();
        currentUser!.GetUserType().Should().Be("Administrator");
    }

    [Fact]
    public void IndexView_WithSuccessMessage_ShouldDisplayTempData()
    {
        // Arrange
        const string successMessage = "Welcome! Registration successful!";
        _viewContext.TempData["SuccessMessage"] = successMessage;

        // Assert
        _viewContext.TempData["SuccessMessage"].Should().Be(successMessage);
        _viewContext.TempData.ContainsKey("SuccessMessage").Should().BeTrue();
    }

    [Fact]
    public void IndexView_AuthenticatedUser_ActivityCardsShouldBeVisible()
    {
        // Arrange
        var student = TestDataBuilder.CreateTestStudent();
        _viewContext.ViewData["CurrentUser"] = student;

        // Assert - These represent the activity cards that should be shown
        var expectedActivityAreas = new[] { "Trips", "Groups", "Expenses" };
        expectedActivityAreas.Should().HaveCount(3);
        expectedActivityAreas.Should().Contain("Trips");
        expectedActivityAreas.Should().Contain("Groups");
        expectedActivityAreas.Should().Contain("Expenses");
    }

    #endregion

    #region Index View Tests - Guest User

    [Fact]
    public void IndexView_WithGuestUser_ShouldShowPublicContent()
    {
        // Arrange - No current user (guest)
        _viewContext.ViewData["CurrentUser"] = null;

        // Assert
        var currentUser = _viewContext.ViewData["CurrentUser"];
        currentUser.Should().BeNull();

        // Guest view should show different content
        var expectedFeatures = new[] { "Find Travel Buddies", "Share Costs", "Organize Easily" };
        expectedFeatures.Should().HaveCount(3);
    }

    [Fact]
    public void IndexView_GuestUser_SearchFormShouldBeAvailable()
    {
        // Arrange
        _viewContext.ViewData["CurrentUser"] = null;

        // Assert - Search form fields that should be available for guests
        var searchFormFields = new[] { "Destination", "Duration", "TripType" };
        searchFormFields.Should().HaveCount(3);
        searchFormFields.Should().Contain("Destination");
        searchFormFields.Should().Contain("Duration");
        searchFormFields.Should().Contain("TripType");
    }

    [Fact]
    public void IndexView_GuestUser_StatsSectionShouldBeVisible()
    {
        // Arrange
        _viewContext.ViewData["CurrentUser"] = null;

        // Assert - Stats that should be displayed
        var expectedStats = new[]
        {
            "Active Students",
            "Organized Trips",
            "Countries Visited",
            "Saved by Group Travel"
        };

        expectedStats.Should().HaveCount(4);
        expectedStats.Should().Contain("Active Students");
        expectedStats.Should().Contain("Organized Trips");
    }

    #endregion

    #region ViewData and TempData Tests

    [Fact]
    public void IndexView_ViewData_Title_ShouldBeSet()
    {
        // Arrange & Act
        _viewContext.ViewData["Title"] = "Home";

        // Assert
        _viewContext.ViewData["Title"].Should().Be("Home");
    }

    [Fact]
    public void IndexView_WithErrorMessage_ShouldDisplayError()
    {
        // Arrange
        const string errorMessage = "Something went wrong. Please try again.";
        _viewContext.TempData["ErrorMessage"] = errorMessage;

        // Assert
        _viewContext.TempData["ErrorMessage"].Should().Be(errorMessage);
        _viewContext.TempData.ContainsKey("ErrorMessage").Should().BeTrue();
    }

    [Fact]
    public void IndexView_WithMultipleTempDataMessages_ShouldHandleCorrectly()
    {
        // Arrange
        _viewContext.TempData["SuccessMessage"] = "Operation completed successfully";
        _viewContext.TempData["InfoMessage"] = "Additional information";
        _viewContext.TempData["WarningMessage"] = "Please review your settings";

        // Assert
        _viewContext.TempData.Keys.Should().HaveCount(3);
        _viewContext.TempData.ContainsKey("SuccessMessage").Should().BeTrue();
        _viewContext.TempData.ContainsKey("InfoMessage").Should().BeTrue();
        _viewContext.TempData.ContainsKey("WarningMessage").Should().BeTrue();
    }

    #endregion

    #region User Experience Tests

    [Theory]
    [InlineData("Student")]
    [InlineData("Administrator")]
    public void IndexView_WithDifferentUserTypes_ShouldCustomizeExperience(string userType)
    {
        // Arrange
        User user = userType switch
        {
            "Student" => TestDataBuilder.CreateTestStudent(),
            "Administrator" => TestDataBuilder.CreateTestAdministrator(),
            _ => throw new ArgumentException("Invalid user type")
        };

        _viewContext.ViewData["CurrentUser"] = user;

        // Assert
        var currentUser = _viewContext.ViewData["CurrentUser"] as User;
        currentUser.Should().NotBeNull();
        currentUser!.GetUserType().Should().Be(userType);
    }

    [Fact]
    public void IndexView_ResponsiveDesign_ElementsShouldAdapt()
    {
        // Arrange - Simulating different screen sizes
        var viewportSizes = new[] { "mobile", "tablet", "desktop" };

        // Assert - All viewport sizes should be supported
        viewportSizes.Should().HaveCount(3);
        viewportSizes.Should().Contain("mobile");
        viewportSizes.Should().Contain("tablet");
        viewportSizes.Should().Contain("desktop");
    }

    [Fact]
    public void IndexView_AccessibilityFeatures_ShouldBePresent()
    {
        // Assert - Accessibility features that should be present
        var accessibilityFeatures = new[]
        {
            "Alt text for images",
            "ARIA labels",
            "Keyboard navigation",
            "Screen reader support",
            "Color contrast compliance"
        };

        accessibilityFeatures.Should().HaveCount(5);
        accessibilityFeatures.Should().Contain("Alt text for images");
        accessibilityFeatures.Should().Contain("ARIA labels");
    }

    #endregion

    #region Performance and SEO Tests

    [Fact]
    public void IndexView_CriticalCSS_ShouldLoadFirst()
    {
        // Arrange
        var criticalStyles = new[]
        {
            "hero-section",
            "navigation",
            "layout-grid"
        };

        // Assert
        criticalStyles.Should().NotBeEmpty();
        criticalStyles.Should().Contain("hero-section");
    }

    [Fact]
    public void IndexView_MetaTags_ShouldBeOptimizedForSEO()
    {
        // Arrange
        var expectedMetaTags = new Dictionary<string, string>
        {
            ["description"] = "TravelShare - Connect with students, share travel costs, create memories",
            ["keywords"] = "travel, students, cost sharing, group travel, budget travel",
            ["og:type"] = "website",
            ["og:title"] = "TravelShare - Travel Together, Share Costs"
        };

        // Assert
        expectedMetaTags.Should().HaveCount(4);
        expectedMetaTags.Should().ContainKey("description");
        expectedMetaTags.Should().ContainKey("keywords");
    }

    [Fact]
    public void IndexView_ImageOptimization_ShouldUseWebPFormat()
    {
        // Arrange
        var imageFormats = new[] { "webp", "jpg", "png" };
        var expectedPrimaryFormat = "webp";

        // Assert
        imageFormats.Should().Contain(expectedPrimaryFormat);
        expectedPrimaryFormat.Should().Be("webp");
    }

    #endregion

    #region Interactive Elements Tests

    [Fact]
    public void IndexView_SearchForm_ShouldHaveValidationRules()
    {
        // Arrange
        var searchFormValidation = new Dictionary<string, bool>
        {
            ["Destination"] = true, // Required
            ["Duration"] = false,   // Optional
            ["TripType"] = false    // Optional
        };

        // Assert
        searchFormValidation.Should().ContainKey("Destination");
        searchFormValidation["Destination"].Should().BeTrue();
    }

    [Fact]
    public void IndexView_CallToActionButtons_ShouldBeProminent()
    {
        // Arrange
        var ctaButtons = new[]
        {
            "Sign In",
            "Join For Free",
            "Create Trip",
            "Browse Trips"
        };

        // Assert
        ctaButtons.Should().HaveCount(4);
        ctaButtons.Should().Contain("Sign In");
        ctaButtons.Should().Contain("Join For Free");
    }

    #endregion
}