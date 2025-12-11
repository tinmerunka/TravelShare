using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;
using TravelShare.ViewModels;

namespace TravelShare.Controllers;
public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthenticationService authService,
        IUserService userService,
        ILogger<AccountController> logger)
    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        // Redirect if already logged in
        if (IsUserLoggedIn())
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.AuthenticateAsync(model.Email, model.Password);

        if (result.IsSuccess && result.User != null)
        {
            // Store user in session
            StoreUserInSession(result.User);
            _logger.LogInformation("User {Email} logged in successfully", model.Email);
            
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Pogrešan email ili lozinka");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        // Redirect if already logged in
        if (IsUserLoggedIn())
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingUser = await _userService.GetUserByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("Email", "Email adresa je veæ registrirana");
            return View(model);
        }

        var newStudent = new Student
        {
            Id = new Random().Next(1000, 9999),
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            StudentId = model.StudentId,
            University = model.University,
            Faculty = model.Faculty,
            PhoneNumber = model.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Preferences = new TravelPreferences
            {
                MinBudget = 200,
                MaxBudget = 1000,
                PreferredTravelType = TravelType.Beach,
                PreferredAccommodation = AccommodationType.Hostel,
                PreferredDestinations = new List<string> { "Hrvatska", "Italija" }
            }
        };

        StoreUserInSession(newStudent);
        _logger.LogInformation("New user {Email} registered successfully", model.Email);

        TempData["SuccessMessage"] = "Registracija uspješna! Dobrodošli u TravelShare!";
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        _authService.Logout();
        HttpContext.Session.Clear();
        
        _logger.LogInformation("User {Email} logged out", userEmail);
        
        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult Profile()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login");
        }

        var viewModel = new UserProfileViewModel
        {
            User = currentUser,
            StudentProfile = currentUser as Student,
            AdminProfile = currentUser as Administrator
        };

        ViewBag.CurrentUser = currentUser;
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Profile");
        }

        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login");
        }

        // Update user properties
        currentUser.FirstName = model.FirstName;
        currentUser.LastName = model.LastName;
        currentUser.Email = model.Email;

        if (currentUser is Student student)
        {
            student.StudentId = model.StudentId;
            student.University = model.University;
            student.Faculty = model.Faculty;
            student.PhoneNumber = model.PhoneNumber;

            if (student.Preferences != null)
            {
                student.Preferences.MinBudget = model.MinBudget ?? student.Preferences.MinBudget;
                student.Preferences.MaxBudget = model.MaxBudget ?? student.Preferences.MaxBudget;

                if (Enum.TryParse<TravelType>(model.PreferredTravelType, out var travelType))
                {
                    student.Preferences.PreferredTravelType = travelType;
                }

                if (Enum.TryParse<AccommodationType>(model.PreferredAccommodation, out var accommodation))
                {
                    student.Preferences.PreferredAccommodation = accommodation;
                }

                if (!string.IsNullOrEmpty(model.PreferredDestinations))
                {
                    student.Preferences.PreferredDestinations = model.PreferredDestinations
                        .Split(',')
                        .Select(d => d.Trim())
                        .Where(d => !string.IsNullOrEmpty(d))
                        .ToList();
                }
            }
        }
        else if (currentUser is Administrator admin)
        {
            admin.Department = model.Department;
        }

        // Update session
        StoreUserInSession(currentUser);

        TempData["SuccessMessage"] = "Profile updated successfully!";
        return RedirectToAction("Profile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please fill all required fields.";
            return RedirectToAction("Profile");
        }

        if (model.NewPassword != model.ConfirmPassword)
        {
            TempData["ErrorMessage"] = "New password and confirmation don't match.";
            return RedirectToAction("Profile");
        }

        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login");
        }

        // In a real app verify the current password
        // For demo, just show success
        TempData["SuccessMessage"] = "Password changed successfully!";
        return RedirectToAction("Profile");
    }


    // Helper Methods

    private bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUser"));
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrEmpty(userJson))
        {
            return null;
        }

        var userType = HttpContext.Session.GetString("UserType");
        
        return userType switch
        {
            "Student" => JsonSerializer.Deserialize<Student>(userJson),
            "Administrator" => JsonSerializer.Deserialize<Administrator>(userJson),
            _ => null
        };
    }

    private void StoreUserInSession(User user)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = false
        };

        var userJson = JsonSerializer.Serialize(user, user.GetType(), options);
        HttpContext.Session.SetString("CurrentUser", userJson);
        HttpContext.Session.SetString("UserType", user.GetUserType());
        HttpContext.Session.SetString("UserEmail", user.Email);
    }

}
