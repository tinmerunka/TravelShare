using System.ComponentModel.DataAnnotations;

namespace TravelShare.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ime je obavezno")]
    [Display(Name = "Ime")]
    [StringLength(50, ErrorMessage = "Ime može imati najviše 50 znakova")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prezime je obavezno")]
    [Display(Name = "Prezime")]
    [StringLength(50, ErrorMessage = "Prezime može imati najviše 50 znakova")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email adresa je obavezna")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu")]
    [Display(Name = "Email adresa")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lozinka je obavezna")]
    [StringLength(100, ErrorMessage = "Lozinka mora imati najmanje {2} znakova", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Lozinka")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potvrda lozinke je obavezna")]
    [DataType(DataType.Password)]
    [Display(Name = "Potvrdi lozinku")]
    [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Broj indeksa je obavezan")]
    [Display(Name = "Broj indeksa")]
    [StringLength(20, ErrorMessage = "Broj indeksa može imati najviše 20 znakova")]
    public string StudentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sveuèilište je obavezno")]
    [Display(Name = "Sveuèilište")]
    [StringLength(100, ErrorMessage = "Naziv sveuèilišta može imati najviše 100 znakova")]
    public string University { get; set; } = string.Empty;

    [Required(ErrorMessage = "Fakultet je obavezan")]
    [Display(Name = "Fakultet")]
    [StringLength(100, ErrorMessage = "Naziv fakulteta može imati najviše 100 znakova")]
    public string Faculty { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Unesite ispravan broj telefona")]
    [Display(Name = "Broj telefona")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Morate prihvatiti uvjete korištenja")]
    [Display(Name = "Prihvaæam uvjete korištenja")]
    public bool AcceptTerms { get; set; }
}
