using FluentAssertions;
using TravelShare.Models.Authentication;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Services.Interfaces;

public class IAuthenticationServiceContractTests
{
    [Fact]
    public void IAuthenticationService_Contract_HasRequiredMethods()
    {
        // Arrange & Assert
        var interfaceType = typeof(IAuthenticationService);

        interfaceType.GetMethod(nameof(IAuthenticationService.AuthenticateAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IAuthenticationService.RegisterUserAsync)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IAuthenticationService.SetCurrentUser)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IAuthenticationService.GetCurrentUser)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IAuthenticationService.Logout)).Should().NotBeNull();
    }

    [Fact]
    public void IAuthenticationService_AuthenticateAsync_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IAuthenticationService).GetMethod(nameof(IAuthenticationService.AuthenticateAsync));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task<AuthenticationResult>));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(string));
        parameters[1].ParameterType.Should().Be(typeof(string));
    }

    [Fact]
    public void IAuthenticationService_RegisterUserAsync_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IAuthenticationService).GetMethod(nameof(IAuthenticationService.RegisterUserAsync));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task<bool>));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(2);
        parameters[0].ParameterType.Should().Be(typeof(Student));
        parameters[1].ParameterType.Should().Be(typeof(string));
    }
}