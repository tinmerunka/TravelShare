using Microsoft.AspNetCore.Mvc;
using TravelShare.Services;
using TravelShare.Services.Trips;

namespace TravelShare.Controllers
{
    public class TripInvitationsController : Controller
    {
        private readonly ITripService _invitationService;

        public TripInvitationsController(ITripService invitationService)
        {
            _invitationService = invitationService;
        }
        [HttpGet]
        public IActionResult Send(int tripId, int userId)
        {
            _invitationService.SendInvitation(tripId, userId);
            return RedirectToAction("Details", "Trips", new { id = tripId });
        }
        [HttpPost]
        public IActionResult Accept(int invitationId)
        {
            _invitationService.AcceptInvitation(invitationId);
            return RedirectToAction("Index", "Trips");
        }
        [HttpPost]
        public IActionResult Decline(int invitationId)
        {
            _invitationService.DeclineInvitation(invitationId);
            return RedirectToAction("Index", "Trips");
        }
    }
}

