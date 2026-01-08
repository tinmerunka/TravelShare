namespace TravelShare.Models.Users;
public class Student : User
{
    public string StudentId { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public TravelPreferences? Preferences { get; set; }
    public string PasswordHash { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public override string GetUserType()
    {
        return "Student";
    }

    public override string GetDisplayName()
    {
        var baseName = base.GetDisplayName().Trim();
        if (string.IsNullOrEmpty(baseName) && string.IsNullOrEmpty(StudentId))
            return string.Empty;
        if (string.IsNullOrEmpty(baseName))
            return $"({StudentId})";
        if (string.IsNullOrEmpty(StudentId))
            return baseName;
        return $"{baseName} ({StudentId})";
    }

    public override string GetUserDescription()
    {
        return $"{GetUserType()} from {University} - {Faculty}";
    }
}