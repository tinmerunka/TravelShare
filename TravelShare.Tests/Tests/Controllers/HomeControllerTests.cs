using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TravelShare.Controllers;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;
using Xunit;

namespace TravelShare.Tests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _controller = new HomeController(_mockLogger.Object, _mockCurrentUserService.Object);
    }

    [Fact]
    public void Index_WithCurrentUser_ReturnsViewWithUserInViewBag()
    {
        // Arrange
        var user = new Student { Id = 1, Email = "test@test.com", FirstName = "John" };
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns(user);

        // Act
        var result = _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.ViewData["CurrentUser"].Should().Be(user);
    }

    [Fact]
    public void Index_WithoutCurrentUser_ReturnsViewWithoutUserInViewBag()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns((User?)null);

        // Act
        var result = _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.ViewData.Should().NotContainKey("CurrentUser");
    }

    [Fact]
    public void Privacy_WithCurrentUser_ReturnsViewWithUserInViewBag()
    {
        // Arrange
        var user = new Student { Id = 1, Email = "test@test.com", FirstName = "John" };
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns(user);

        // Act
        var result = _controller.Privacy();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.ViewData["CurrentUser"].Should().Be(user);
    }

    [Fact]
    public void Privacy_WithoutCurrentUser_ReturnsViewWithoutUserInViewBag()
    {
        // Arrange
        _mockCurrentUserService.Setup(x => x.GetCurrentUser()).Returns((User?)null);

        // Act
        var result = _controller.Privacy();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.ViewData.Should().NotContainKey("CurrentUser");
    }
}