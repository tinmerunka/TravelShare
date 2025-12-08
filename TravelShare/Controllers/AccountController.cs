using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelShare.Models.Users;
using TravelShare.Services;
using TravelShare.ViewModels;

namespace TravelShare.Controllers;

/// <summary>
/// Controller for user account management - demonstrates clean MVC architecture
/// </summary>
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

    #region Login

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

    #endregion

    #region Logout

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

    #endregion

    #region Profile

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

    #endregion

    #region Helper Methods

    private bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUser"));
    }

    private UserBase? GetCurrentUser()
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

    private void StoreUserInSession(UserBase user)
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

    #endregion
}
