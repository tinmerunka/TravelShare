using System.ComponentModel.DataAnnotations;

namespace TravelShare.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Student specific fields
        public string? StudentId { get; set; }
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public string? PhoneNumber { get; set; }

        // Admin specific fields
        public string? Department { get; set; }

        // Travel preferences
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string? PreferredTravelType { get; set; }
        public string? PreferredAccommodation { get; set; }
        public string? PreferredDestinations { get; set; }
    }
}