using Microsoft.AspNetCore.Mvc;
using TravelShare.Services;
using TravelShare.Services.Trips;
using TravelShare.ViewModels.Trips;

namespace TravelShare.Controllers
{
    public class TripMediaController : Controller
    {
        private readonly ITripService _mediaService;

        public TripMediaController(ITripService mediaService)
        {
            _mediaService = mediaService;
        }
        [HttpGet]
        public IActionResult Add(int tripId)
        {
            return View(new TripMediaViewModel { MediaId = tripId });
        }

        [HttpPost]
        public IActionResult Add(TripMediaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _mediaService.AddMedia(model);
            return RedirectToAction("Details", "Trips", new { id = model.TripId });
        }
        [HttpDelete]
        public IActionResult Delete(int id, int tripId)
        {
            _mediaService.DeleteMedia(id);
            return RedirectToAction("Details", "Trips", new { id = tripId });
        }
    }
}

