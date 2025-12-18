namespace TravelShare.Models.Users;
public abstract class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual string GetDisplayName()
    {
        return $"{FirstName} {LastName}";
    }

    public abstract string GetUserType();

    public virtual string GetUserDescription()
    {
        return $"{GetUserType()}: {GetDisplayName()}";
    }
}
