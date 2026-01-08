using FluentAssertions;
using TravelShare.Models.Users;
using Xunit;

namespace TravelShare.Tests.Models.Users;

public class TravelPreferencesTests
{
    [Fact]
    public void TravelPreferences_DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var preferences = new TravelPreferences();

        // Assert
        preferences.Should().NotBeNull();
        preferences.PreferredDestinations.Should().NotBeNull();
        preferences.PreferredDestinations.Should().BeEmpty();
        preferences.MinBudget.Should().Be(0);
        preferences.MaxBudget.Should().Be(0);
    }

    [Fact]
    public void TravelPreferences_WithCustomValues_StoresCorrectly()
    {
        // Arrange & Act
        var preferences = new TravelPreferences
        {
            MinBudget = 200,
            MaxBudget = 1000,
            PreferredTravelType = TravelType.Cultural,
            PreferredAccommodation = AccommodationType.Hotel,
            PreferredDestinations = new List<string> { "Paris", "Rome", "Barcelona" }
        };

        // Assert
        preferences.MinBudget.Should().Be(200);
        preferences.MaxBudget.Should().Be(1000);
        preferences.PreferredTravelType.Should().Be(TravelType.Cultural);
        preferences.PreferredAccommodation.Should().Be(AccommodationType.Hotel);
        preferences.PreferredDestinations.Should().BeEquivalentTo(new[] { "Paris", "Rome", "Barcelona" });
    }

    [Theory]
    [InlineData(TravelType.Adventure)]
    [InlineData(TravelType.Beach)]
    [InlineData(TravelType.Cultural)]
    [InlineData(TravelType.Backpacking)]
    [InlineData(TravelType.Luxury)]
    [InlineData(TravelType.Roadtrip)]
    [InlineData(TravelType.Festival)]
    public void PreferredTravelType_AcceptsAllEnumValues(TravelType travelType)
    {
        // Arrange
        var preferences = new TravelPreferences();

        // Act
        preferences.PreferredTravelType = travelType;

        // Assert
        preferences.PreferredTravelType.Should().Be(travelType);
    }

    [Theory]
    [InlineData(AccommodationType.Hostel)]
    [InlineData(AccommodationType.Hotel)]
    [InlineData(AccommodationType.Apartment)]
    [InlineData(AccommodationType.Camping)]
    [InlineData(AccommodationType.Mixed)]
    public void PreferredAccommodation_AcceptsAllEnumValues(AccommodationType accommodation)
    {
        // Arrange
        var preferences = new TravelPreferences();

        // Act
        preferences.PreferredAccommodation = accommodation;

        // Assert
        preferences.PreferredAccommodation.Should().Be(accommodation);
    }

    [Fact]
    public void PreferredDestinations_CanAddAndRemoveItems()
    {
        // Arrange
        var preferences = new TravelPreferences();

        // Act
        preferences.PreferredDestinations.Add("Tokyo");
        preferences.PreferredDestinations.Add("Seoul");

        // Assert
        preferences.PreferredDestinations.Should().HaveCount(2);
        preferences.PreferredDestinations.Should().Contain("Tokyo");
        preferences.PreferredDestinations.Should().Contain("Seoul");

        // Act
        preferences.PreferredDestinations.Remove("Tokyo");

        // Assert
        preferences.PreferredDestinations.Should().HaveCount(1);
        preferences.PreferredDestinations.Should().NotContain("Tokyo");
        preferences.PreferredDestinations.Should().Contain("Seoul");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(100, 500)]
    [InlineData(1000, 5000)]
    [InlineData(50.50, 299.99)]
    public void Budget_AcceptsDecimalValues(decimal minBudget, decimal maxBudget)
    {
        // Arrange
        var preferences = new TravelPreferences();

        // Act
        preferences.MinBudget = minBudget;
        preferences.MaxBudget = maxBudget;

        // Assert
        preferences.MinBudget.Should().Be(minBudget);
        preferences.MaxBudget.Should().Be(maxBudget);
    }
}