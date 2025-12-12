using Microsoft.AspNetCore.Mvc;
using TravelShare.Services;
using TravelShare.Services.Trips;

namespace TravelShare.Controllers
{
    public class TripMembersController : Controller
    {
        private readonly ITripService _tripService;

        public TripMembersController(ITripService tripService)
        {
            _tripService = tripService;
        }
        [HttpPost]
        public IActionResult AddMember(int tripId, int userId)
        {
            _tripService.AddMember(tripId, userId);
            return RedirectToAction("Details", "Trips", new { id = tripId });
        }
        [HttpDelete]
        public IActionResult RemoveMember(int tripId, int userId)
        {
            _tripService.RemoveMember(tripId, userId);
            return RedirectToAction("Details", "Trips", new { id = tripId });
        }
    }
}
