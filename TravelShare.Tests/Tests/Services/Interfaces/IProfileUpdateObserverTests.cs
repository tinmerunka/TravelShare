using FluentAssertions;
using Moq;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;
using TravelShare.Tests.TestHelpers;
using Xunit;

namespace TravelShare.Tests.Services.Interfaces;

public class IProfileUpdateObserverContractTests
{
    [Fact]
    public void IProfileUpdateObserver_Contract_HasRequiredMethods()
    {
        // Arrange & Assert
        var interfaceType = typeof(IProfileUpdateObserver);

        interfaceType.GetMethod(nameof(IProfileUpdateObserver.OnProfileUpdatedAsync)).Should().NotBeNull();
    }

    [Fact]
    public void IProfileUpdateObserver_OnProfileUpdatedAsync_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IProfileUpdateObserver).GetMethod(nameof(IProfileUpdateObserver.OnProfileUpdatedAsync));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(Task));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(User));
    }
}