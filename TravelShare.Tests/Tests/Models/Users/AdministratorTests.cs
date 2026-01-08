using FluentAssertions;
using TravelShare.Models.Users;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Models.Users;

public class AdministratorTests
{
    [Fact]
    public void GetUserType_ReturnsAdministrator()
    {
        // Arrange
        var admin = TestDataBuilder.CreateTestAdministrator();

        // Act
        var userType = admin.GetUserType();

        // Assert
        userType.Should().Be("Administrator");
    }

    [Fact]
    public void GetUserDescription_ReturnsAdminDescription()
    {
        // Arrange
        var admin = new Administrator
        {
            FirstName = "Jane",
            LastName = "Smith",
            Department = "IT"
        };

        // Act
        var description = admin.GetUserDescription();

        // Assert
        description.Should().Contain("Administrator");
        description.Should().Contain("IT");
    }

    [Fact]
    public void HasPermission_WithExistingPermission_ReturnsTrue()
    {
        // Arrange
        var admin = new Administrator
        {
            Permissions = new List<string> { "ManageUsers", "ViewReports", "DeleteData" }
        };

        // Act & Assert
        admin.HasPermission("ManageUsers").Should().BeTrue();
        admin.HasPermission("ViewReports").Should().BeTrue();
        admin.HasPermission("DeleteData").Should().BeTrue();
    }

    [Fact]
    public void HasPermission_WithNonExistingPermission_ReturnsFalse()
    {
        // Arrange
        var admin = new Administrator
        {
            Permissions = new List<string> { "ManageUsers", "ViewReports" }
        };

        // Act & Assert
        admin.HasPermission("DeleteData").Should().BeFalse();
        admin.HasPermission("AdminAccess").Should().BeFalse();
    }

    [Fact]
    public void HasPermission_WithNullPermissions_ReturnsFalse()
    {
        // Arrange
        var admin = new Administrator
        {
            Permissions = null!
        };

        // Act & Assert
        admin.HasPermission("ManageUsers").Should().BeFalse();
    }

    [Fact]
    public void HasPermission_WithEmptyPermissions_ReturnsFalse()
    {
        // Arrange
        var admin = new Administrator
        {
            Permissions = new List<string>()
        };

        // Act & Assert
        admin.HasPermission("ManageUsers").Should().BeFalse();
    }

    [Theory]
    [InlineData("ManageUsers")]
    [InlineData("manageusers")]
    [InlineData("MANAGEUSERS")]
    public void HasPermission_IsCaseSensitive(string permission)
    {
        // Arrange
        var admin = new Administrator
        {
            Permissions = new List<string> { "ManageUsers" }
        };

        // Act & Assert
        if (permission == "ManageUsers")
        {
            admin.HasPermission(permission).Should().BeTrue();
        }
        else
        {
            admin.HasPermission(permission).Should().BeFalse();
        }
    }
}