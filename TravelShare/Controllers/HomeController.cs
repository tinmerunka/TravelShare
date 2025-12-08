using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelShare.Models;
using TravelShare.Models.Users;

namespace TravelShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            LoadCurrentUser();
            return View();
        }

        public IActionResult Privacy()
        {
            LoadCurrentUser();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void LoadCurrentUser()
        {
            var userJson = HttpContext.Session.GetString("CurrentUser");
            if (!string.IsNullOrEmpty(userJson))
            {
                var userType = HttpContext.Session.GetString("UserType");
                UserBase? currentUser = null;

                if (userType == "Student")
                {
                    currentUser = JsonSerializer.Deserialize<Student>(userJson);
                }
                else if (userType == "Administrator")
                {
                    currentUser = JsonSerializer.Deserialize<Administrator>(userJson);
                }

                if (currentUser != null)
                {
                    ViewBag.CurrentUser = currentUser;
                }
            }
        }
    }
}
