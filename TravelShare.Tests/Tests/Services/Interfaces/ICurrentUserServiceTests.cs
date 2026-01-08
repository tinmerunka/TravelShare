using FluentAssertions;
using TravelShare.Services.Interfaces;
using Xunit;

namespace TravelShare.Tests.Services.Interfaces;

// This is more of a contract test - testing implementations rather than the interface itself
public class ICurrentUserServiceContractTests
{
    [Fact]
    public void ICurrentUserService_Contract_HasRequiredMethods()
    {
        // Arrange & Assert
        var interfaceType = typeof(ICurrentUserService);

        interfaceType.GetMethod(nameof(ICurrentUserService.GetCurrentUser)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(ICurrentUserService.StoreCurrentUser)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(ICurrentUserService.IsUserLoggedIn)).Should().NotBeNull();
        interfaceType.GetMethod(nameof(ICurrentUserService.ClearCurrentUser)).Should().NotBeNull();
    }
}