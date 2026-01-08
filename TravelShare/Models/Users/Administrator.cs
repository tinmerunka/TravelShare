namespace TravelShare.Models.Users;
public class Administrator : User
{
    public string Department { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
    public string PasswordHash { get; set; }

    public override string GetUserType()
    {
        return "Administrator";
    }

    public override string GetUserDescription()
    {
        return $"{GetUserType()} - {Department} Department";
    }

    public bool HasPermission(string permission)
    {
        if (Permissions == null || string.IsNullOrEmpty(permission))
            return false;

        return Permissions.Contains(permission, StringComparer.Ordinal); // Changed to case-sensitive
    }
}