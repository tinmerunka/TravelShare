using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using System.Text.Json;
using TravelShare.Models.Users;
using TravelShare.Services;
using TravelShare.Services.Interfaces;
using Xunit;

namespace TravelShare.Tests.Services;

public class SessionCurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ISession> _mockSession;
    private readonly SessionCurrentUserService _service;
    private readonly Dictionary<string, byte[]> _sessionStorage;

    public SessionCurrentUserServiceTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockHttpContext = new Mock<HttpContext>();
        _mockSession = new Mock<ISession>();
        _sessionStorage = new Dictionary<string, byte[]>();

        // Setup session to use dictionary-based storage
        _mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => _sessionStorage[key] = value);

        _mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                var exists = _sessionStorage.TryGetValue(key, out var storedValue);
                value = storedValue ?? Array.Empty<byte>();
                return exists;
            });

        _mockSession.Setup(s => s.Remove(It.IsAny<string>()))
            .Callback<string>(key => _sessionStorage.Remove(key));

        _mockHttpContext.Setup(c => c.Session).Returns(_mockSession.Object);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(_mockHttpContext.Object);

        _service = new SessionCurrentUserService(_mockHttpContextAccessor.Object);
    }

    #region GetCurrentUser Tests

    [Fact]
    public void GetCurrentUser_WhenNoUserInSession_ReturnsNull()
    {
        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WhenSessionIsNull_ReturnsNull()
    {
        // Arrange
        _mockHttpContext.Setup(c => c.Session).Returns((ISession)null!);

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WhenHttpContextIsNull_ReturnsNull()
    {
        // Arrange
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext)null!);

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WithValidStudentInSession_ReturnsStudent()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash123",
            StudentId = "S12345",
            University = "Test University",
            Faculty = "Engineering",
            Preferences = new TravelPreferences
            {
                MinBudget = 100,
                MaxBudget = 500,
                PreferredTravelType = TravelType.Adventure,
                PreferredAccommodation = AccommodationType.Hostel
            }
        };

        var studentJson = JsonSerializer.Serialize(student, new JsonSerializerOptions { WriteIndented = false });
        _sessionStorage["CurrentUser"] = Encoding.UTF8.GetBytes(studentJson);
        _sessionStorage["UserType"] = Encoding.UTF8.GetBytes("Student");

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Student>();
        var resultStudent = result as Student;
        resultStudent!.Id.Should().Be(1);
        resultStudent.Email.Should().Be("student@test.com");
        resultStudent.FirstName.Should().Be("John");
        resultStudent.Preferences.Should().NotBeNull();
        resultStudent.Preferences!.MinBudget.Should().Be(100);
    }

    [Fact]
    public void GetCurrentUser_WithValidAdministratorInSession_ReturnsAdministrator()
    {
        // Arrange
        var admin = new Administrator
        {
            Id = 2,
            Email = "admin@test.com",
            FirstName = "Jane",
            LastName = "Admin",
            PasswordHash = "hash456",
            Department = "IT",
            Permissions = new List<string> { "ManageUsers", "ViewReports" }
        };

        var adminJson = JsonSerializer.Serialize(admin, new JsonSerializerOptions { WriteIndented = false });
        _sessionStorage["CurrentUser"] = Encoding.UTF8.GetBytes(adminJson);
        _sessionStorage["UserType"] = Encoding.UTF8.GetBytes("Administrator");

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Administrator>();
        var resultAdmin = result as Administrator;
        resultAdmin!.Id.Should().Be(2);
        resultAdmin.Email.Should().Be("admin@test.com");
        resultAdmin.Department.Should().Be("IT");
        resultAdmin.Permissions.Should().BeEquivalentTo(new[] { "ManageUsers", "ViewReports" });
    }

    [Fact]
    public void GetCurrentUser_WithUnknownUserType_ReturnsNull()
    {
        // Arrange
        var userJson = JsonSerializer.Serialize(new { Id = 1, Email = "test@test.com" });
        _sessionStorage["CurrentUser"] = Encoding.UTF8.GetBytes(userJson);
        _sessionStorage["UserType"] = Encoding.UTF8.GetBytes("UnknownType");

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WithMalformedJson_ReturnsNull()
    {
        // Arrange
        _sessionStorage["CurrentUser"] = Encoding.UTF8.GetBytes("{invalid json}");
        _sessionStorage["UserType"] = Encoding.UTF8.GetBytes("Student");

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WithEmptyUserJson_ReturnsNull()
    {
        // Arrange
        _sessionStorage["CurrentUser"] = Encoding.UTF8.GetBytes("");
        _sessionStorage["UserType"] = Encoding.UTF8.GetBytes("Student");

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WithUserTypeButNoUserData_ReturnsNull()
    {
        // Arrange
        _sessionStorage["UserType"] = Encoding.UTF8.GetBytes("Student");
        // No CurrentUser in session

        // Act
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region StoreCurrentUser Tests

    [Fact]
    public void StoreCurrentUser_WithStudent_StoresCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash123",
            StudentId = "S12345",
            University = "Test University",
            Faculty = "Engineering",
            Preferences = new TravelPreferences
            {
                MinBudget = 100,
                MaxBudget = 500,
                PreferredTravelType = TravelType.Adventure,
                PreferredAccommodation = AccommodationType.Hostel
            }
        };

        // Act
        _service.StoreCurrentUser(student);

        // Assert
        _sessionStorage.Should().ContainKey("CurrentUser");
        _sessionStorage.Should().ContainKey("UserType");
        _sessionStorage.Should().ContainKey("UserEmail");

        var storedUserJson = Encoding.UTF8.GetString(_sessionStorage["CurrentUser"]);
        var storedUserType = Encoding.UTF8.GetString(_sessionStorage["UserType"]);
        var storedEmail = Encoding.UTF8.GetString(_sessionStorage["UserEmail"]);

        storedUserType.Should().Be("Student");
        storedEmail.Should().Be("student@test.com");

        var deserializedStudent = JsonSerializer.Deserialize<Student>(storedUserJson);
        deserializedStudent.Should().NotBeNull();
        deserializedStudent!.Id.Should().Be(1);
        deserializedStudent.Email.Should().Be("student@test.com");
    }

    [Fact]
    public void StoreCurrentUser_WithAdministrator_StoresCorrectly()
    {
        // Arrange
        var admin = new Administrator
        {
            Id = 2,
            Email = "admin@test.com",
            FirstName = "Jane",
            LastName = "Admin",
            PasswordHash = "hash456",
            Department = "IT",
            Permissions = new List<string> { "ManageUsers" }
        };

        // Act
        _service.StoreCurrentUser(admin);

        // Assert
        var storedUserType = Encoding.UTF8.GetString(_sessionStorage["UserType"]);
        var storedEmail = Encoding.UTF8.GetString(_sessionStorage["UserEmail"]);

        storedUserType.Should().Be("Administrator");
        storedEmail.Should().Be("admin@test.com");
    }

    [Fact]
    public void StoreCurrentUser_WhenSessionIsNull_DoesNotThrow()
    {
        // Arrange
        _mockHttpContext.Setup(c => c.Session).Returns((ISession)null!);
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };

        // Act
        Action act = () => _service.StoreCurrentUser(student);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void StoreCurrentUser_OverwritesPreviousUser()
    {
        // Arrange
        var student1 = new Student
        {
            Id = 1,
            Email = "student1@test.com",
            FirstName = "First",
            LastName = "Student",
            PasswordHash = "hash1",
            StudentId = "S001"
        };

        var student2 = new Student
        {
            Id = 2,
            Email = "student2@test.com",
            FirstName = "Second",
            LastName = "Student",
            PasswordHash = "hash2",
            StudentId = "S002"
        };

        // Act
        _service.StoreCurrentUser(student1);
        _service.StoreCurrentUser(student2);

        // Assert
        var storedEmail = Encoding.UTF8.GetString(_sessionStorage["UserEmail"]);
        storedEmail.Should().Be("student2@test.com");

        var result = _service.GetCurrentUser();
        result.Should().NotBeNull();
        result!.Id.Should().Be(2);
        result.Email.Should().Be("student2@test.com");
    }

    [Fact]
    public void StoreCurrentUser_PreservesAllUserProperties()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash123",
            StudentId = "S12345",
            University = "Test University",
            Faculty = "Engineering",
            PhoneNumber = "1234567890",
            DateOfBirth = new DateTime(2000, 1, 1),
            Preferences = new TravelPreferences
            {
                MinBudget = 100,
                MaxBudget = 500,
                PreferredTravelType = TravelType.Cultural,
                PreferredAccommodation = AccommodationType.Hotel,
                PreferredDestinations = new List<string> { "Paris", "Rome" }
            }
        };

        // Act
        _service.StoreCurrentUser(student);
        var retrieved = _service.GetCurrentUser() as Student;

        // Assert
        retrieved.Should().NotBeNull();
        retrieved!.Id.Should().Be(student.Id);
        retrieved.Email.Should().Be(student.Email);
        retrieved.FirstName.Should().Be(student.FirstName);
        retrieved.LastName.Should().Be(student.LastName);
        retrieved.PhoneNumber.Should().Be(student.PhoneNumber);
        retrieved.DateOfBirth.Should().Be(student.DateOfBirth);
        retrieved.Preferences.Should().NotBeNull();
        retrieved.Preferences!.MinBudget.Should().Be(100);
        retrieved.Preferences.MaxBudget.Should().Be(500);
        retrieved.Preferences.PreferredDestinations.Should().BeEquivalentTo(new[] { "Paris", "Rome" });
    }

    #endregion

    #region IsUserLoggedIn Tests

    [Fact]
    public void IsUserLoggedIn_WhenUserInSession_ReturnsTrue()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };
        _service.StoreCurrentUser(student);

        // Act
        var result = _service.IsUserLoggedIn();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsUserLoggedIn_WhenNoUserInSession_ReturnsFalse()
    {
        // Act
        var result = _service.IsUserLoggedIn();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsUserLoggedIn_WhenSessionIsNull_ReturnsFalse()
    {
        // Arrange
        _mockHttpContext.Setup(c => c.Session).Returns((ISession)null!);

        // Act
        var result = _service.IsUserLoggedIn();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsUserLoggedIn_AfterClearingUser_ReturnsFalse()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };
        _service.StoreCurrentUser(student);

        // Act
        _service.ClearCurrentUser();
        var result = _service.IsUserLoggedIn();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsUserLoggedIn_WithEmptyCurrentUser_ReturnsFalse()
    {
        // Arrange
        _sessionStorage["CurrentUser"] = Encoding.UTF8.GetBytes("");

        // Act
        var result = _service.IsUserLoggedIn();

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region ClearCurrentUser Tests

    [Fact]
    public void ClearCurrentUser_RemovesAllUserData()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };
        _service.StoreCurrentUser(student);

        // Act
        _service.ClearCurrentUser();

        // Assert
        _sessionStorage.Should().NotContainKey("CurrentUser");
        _sessionStorage.Should().NotContainKey("UserType");
        _sessionStorage.Should().NotContainKey("UserEmail");
    }

    [Fact]
    public void ClearCurrentUser_WhenSessionIsNull_DoesNotThrow()
    {
        // Arrange
        _mockHttpContext.Setup(c => c.Session).Returns((ISession)null!);

        // Act
        Action act = () => _service.ClearCurrentUser();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ClearCurrentUser_WhenNoUserStored_DoesNotThrow()
    {
        // Act
        Action act = () => _service.ClearCurrentUser();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ClearCurrentUser_AfterClearing_GetCurrentUserReturnsNull()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };
        _service.StoreCurrentUser(student);

        // Act
        _service.ClearCurrentUser();
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Integration/Workflow Tests

    [Fact]
    public void FullWorkflow_StoreRetrieveClear_WorksCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash123",
            StudentId = "S12345",
            University = "Test University",
            Faculty = "Engineering",
            Preferences = new TravelPreferences
            {
                MinBudget = 100,
                MaxBudget = 500
            }
        };

        // Act & Assert - Store
        _service.StoreCurrentUser(student);
        _service.IsUserLoggedIn().Should().BeTrue();

        // Act & Assert - Retrieve
        var retrieved = _service.GetCurrentUser();
        retrieved.Should().NotBeNull();
        retrieved.Should().BeOfType<Student>();
        retrieved!.Email.Should().Be("student@test.com");

        // Act & Assert - Clear
        _service.ClearCurrentUser();
        _service.IsUserLoggedIn().Should().BeFalse();
        _service.GetCurrentUser().Should().BeNull();
    }

    [Fact]
    public void SwitchUser_StoresNewUserCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "student@test.com",
            FirstName = "Student",
            LastName = "User",
            PasswordHash = "hash1",
            StudentId = "S001"
        };

        var admin = new Administrator
        {
            Id = 2,
            Email = "admin@test.com",
            FirstName = "Admin",
            LastName = "User",
            PasswordHash = "hash2",
            Department = "IT",
            Permissions = new List<string> { "ManageUsers" }
        };

        // Act
        _service.StoreCurrentUser(student);
        var firstUser = _service.GetCurrentUser();

        _service.StoreCurrentUser(admin);
        var secondUser = _service.GetCurrentUser();

        // Assert
        firstUser.Should().BeOfType<Student>();
        firstUser!.Email.Should().Be("student@test.com");

        secondUser.Should().BeOfType<Administrator>();
        secondUser!.Email.Should().Be("admin@test.com");
    }

    [Fact]
    public void MultipleOperations_MaintainConsistency()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };

        // Act & Assert
        _service.IsUserLoggedIn().Should().BeFalse();

        _service.StoreCurrentUser(student);
        _service.IsUserLoggedIn().Should().BeTrue();
        _service.GetCurrentUser().Should().NotBeNull();

        _service.ClearCurrentUser();
        _service.IsUserLoggedIn().Should().BeFalse();
        _service.GetCurrentUser().Should().BeNull();

        _service.StoreCurrentUser(student);
        _service.IsUserLoggedIn().Should().BeTrue();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void GetCurrentUser_WithNullPreferences_HandlesGracefully()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001",
            Preferences = null
        };

        // Act
        _service.StoreCurrentUser(student);
        var result = _service.GetCurrentUser() as Student;

        // Assert
        result.Should().NotBeNull();
        result!.Preferences.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_WithEmptyPermissions_HandlesGracefully()
    {
        // Arrange
        var admin = new Administrator
        {
            Id = 1,
            Email = "admin@test.com",
            FirstName = "Admin",
            LastName = "User",
            PasswordHash = "hash",
            Department = "IT",
            Permissions = new List<string>()
        };

        // Act
        _service.StoreCurrentUser(admin);
        var result = _service.GetCurrentUser() as Administrator;

        // Assert
        result.Should().NotBeNull();
        result!.Permissions.Should().BeEmpty();
    }

    [Fact]
    public void StoreCurrentUser_WithSpecialCharactersInEmail_HandlesCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            Email = "test+tag@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash",
            StudentId = "S001"
        };

        // Act
        _service.StoreCurrentUser(student);
        var result = _service.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test+tag@example.com");
    }

    #endregion
}