namespace TravelShare.Models.Users;

/// <summary>
/// Represents a student user - inherits from UserBase
/// </summary>
public class Student : UserBase
{
    public string StudentId { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public TravelPreferences? Preferences { get; set; }

    public override string GetUserType()
    {
        return "Student";
    }

    public override string GetDisplayName()
    {
        return $"{base.GetDisplayName()} ({StudentId})";
    }

    public override string GetUserDescription()
    {
        return $"{GetUserType()} from {University} - {Faculty}";
    }
}
