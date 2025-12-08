namespace TravelShare.Models.Users;

/// <summary>
/// Represents user travel preferences
/// </summary>
public class TravelPreferences
{
    public decimal MaxBudget { get; set; }
    public decimal MinBudget { get; set; }
    public List<string> PreferredDestinations { get; set; } = new();
    public TravelType PreferredTravelType { get; set; }
    public AccommodationType PreferredAccommodation { get; set; }
}

public enum TravelType
{
    Adventure,
    Cultural,
    Beach,
    Backpacking,
    Luxury,
    Roadtrip,
    Festival
}

public enum AccommodationType
{
    Hostel,
    Hotel,
    Apartment,
    Camping,
    Mixed
}
