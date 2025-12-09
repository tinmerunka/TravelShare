using System.ComponentModel.DataAnnotations;

namespace TravelShare.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password must be at least {2} characters long", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Student ID is required")]
    [Display(Name = "Student ID")]
    [StringLength(20, ErrorMessage = "Student ID cannot exceed 20 characters")]
    public string StudentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "University is required")]
    [Display(Name = "University")]
    [StringLength(100, ErrorMessage = "University name cannot exceed 100 characters")]
    public string University { get; set; } = string.Empty;

    [Required(ErrorMessage = "Faculty is required")]
    [Display(Name = "Faculty")]
    [StringLength(100, ErrorMessage = "Faculty name cannot exceed 100 characters")]
    public string Faculty { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "You must accept the terms of use")]
    [Display(Name = "I accept the terms of use")]
    public bool AcceptTerms { get; set; }
}
