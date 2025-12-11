using Microsoft.AspNetCore.Mvc;
using TravelShare.Services.Groups;
using TravelShare.ViewModels.SocialGroups;

namespace TravelShare.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public IActionResult Index()
        {
            var groups = _groupService.GetAllGroups();
            return View(groups);
        }

        public IActionResult Details(int id)
        {
            var group = _groupService.GetGroupById(id);
            if (group == null)
                return NotFound();

            return View(group);
        }

        public IActionResult Create()
        {
            return View(new GroupViewModel());
        }

        [HttpPost]
        public IActionResult Create(GroupViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _groupService.CreateGroup(model);
            return RedirectToAction("Index");
        }
    }
}

