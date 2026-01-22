using Microsoft.AspNetCore.Mvc;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;

namespace TravelShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrentUserService _currentUserService;

        public HomeController(ILogger<HomeController> logger, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public IActionResult Index()
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                ViewBag.CurrentUser = currentUser;
            }
            return View();
        }

        public IActionResult Privacy()
{
    var currentUser = _currentUserService.GetCurrentUser();
    if (currentUser != null)
    {
        ViewBag.CurrentUser = currentUser;
    }
    
    // Add privacy-specific data
    ViewBag.PageTitle = "Privacy Policy";
    ViewBag.ContactEmail = "privacy@travelshare.com";
    
    return View();
}
    }
}