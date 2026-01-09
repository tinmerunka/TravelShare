using Microsoft.AspNetCore.Mvc;
using TravelShare.Models.Trips;
using TravelShare.Services;
using TravelShare.Services.Trips;
using TravelShare.ViewModels.Trips;

namespace TravelShare.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService _tripService;     

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var trips = _tripService.GetAllTrips();
            return View(trips);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var trip = _tripService.GetTripById(id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TripCreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(TripViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _tripService.CreateTrip(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vm = _tripService.GetTripForEdit(id);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(TripEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _tripService.UpdateTrip(model);
            return RedirectToAction("Details", new { id = model.Id });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _tripService.DeleteTrip(id);
            return RedirectToAction("Index");
        }
    }

    public class TripEditViewModel
    {
        public object Id { get; internal set; }
    }

    internal class TripCreateViewModel
    {
        public TripCreateViewModel()
        {
        }
    }
}

