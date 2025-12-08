namespace TravelShare.Models.Users;

/// <summary>
/// Base class for all user types in the system
/// Demonstrates inheritance - all users share common properties
/// </summary>
public abstract class UserBase
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Virtual method for polymorphism - each user type can override
    /// </summary>
    public virtual string GetDisplayName()
    {
        return $"{FirstName} {LastName}";
    }

    /// <summary>
    /// Abstract method - must be implemented by derived classes
    /// </summary>
    public abstract string GetUserType();

    /// <summary>
    /// Virtual method to get user's full description
    /// </summary>
    public virtual string GetUserDescription()
    {
        return $"{GetUserType()}: {GetDisplayName()}";
    }
}
