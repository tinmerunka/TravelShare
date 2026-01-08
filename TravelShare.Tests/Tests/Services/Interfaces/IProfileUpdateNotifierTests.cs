// TravelShare.Tests/Services/Interfaces/IProfileUpdateNotifierTests.cs
using FluentAssertions;
using TravelShare.Services.Interfaces;
using Xunit;

namespace TravelShare.Tests.Services.Interfaces;

public class IProfileUpdateNotifierContractTests
{
    [Fact]
    public void IProfileUpdateNotifier_Contract_HasRequiredMethods()
    {
        // Arrange & Assert
        var interfaceType = typeof(IProfileUpdateNotifier);

        interfaceType.GetMethod(nameof(IProfileUpdateNotifier.Subscribe)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(IProfileUpdateNotifier.Unsubscribe)).Should().NotBeNull();
    }

    [Fact]
    public void IProfileUpdateNotifier_Subscribe_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IProfileUpdateNotifier).GetMethod(nameof(IProfileUpdateNotifier.Subscribe));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(void));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(IProfileUpdateObserver));
    }

    [Fact]
    public void IProfileUpdateNotifier_Unsubscribe_HasCorrectSignature()
    {
        // Arrange
        var method = typeof(IProfileUpdateNotifier).GetMethod(nameof(IProfileUpdateNotifier.Unsubscribe));

        // Assert
        method.Should().NotBeNull();
        method!.ReturnType.Should().Be(typeof(void));

        var parameters = method.GetParameters();
        parameters.Should().HaveCount(1);
        parameters[0].ParameterType.Should().Be(typeof(IProfileUpdateObserver));
    }
}