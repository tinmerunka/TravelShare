// TravelShare.Tests/Services/Interfaces/IUserServiceTests.cs
using FluentAssertions;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;
using Xunit;

namespace TravelShare.Tests.Services.Interfaces;

public class IUserServiceContractTests
{
    [Fact]
    public void IUserService_Contract_HasRequiredMethods()
    {
        // Arrange & Assert
        var interfaceType = typeof(IUserService);

        interfaceType.GetMethod(nameof(IUserService.GetUserByIdAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IUserService.GetUserByEmailAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IUserService.UpdateUserProfileAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IUserService.GetAllUsersAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IUserService.ResetPasswordAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IUserService.UpdateTravelPreferencesAsync)).Should().NotBeNull();
    }

    [Fact]
    public void IUserService_GetUserByIdAsync_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IUserService).GetMethod(nameof(IUserService.GetUserByIdAsync));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task<User?>));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(int));
    }

    [Fact]
    public void IUserService_GetUserByEmailAsync_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IUserService).GetMethod(nameof(IUserService.GetUserByEmailAsync));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task<User?>));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(string));
    }

    [Fact]
    public void IUserService_UpdateTravelPreferencesAsync_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IUserService).GetMethod(nameof(IUserService.UpdateTravelPreferencesAsync));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task<bool>));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(int));
        parameters[1].ParameterType.Should().Be(typeof(TravelPreferences));
    }
}