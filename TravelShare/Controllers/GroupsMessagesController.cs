using Microsoft.AspNetCore.Mvc;
using TravelShare.Services.Groups;
using TravelShare.ViewModels.SocialGroups;

namespace TravelShare.Controllers
{
    public class GroupMessagesController : Controller
    {
        private readonly IGroupService _messageService;

        public GroupMessagesController(IGroupService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet]
        public IActionResult Send(int groupId)
        {
            return View(new GroupMessageViewModel { GroupId = groupId });
        }

        [HttpPost]
        public IActionResult Send(GroupMessageViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _messageService.SendMessage(model);
            return RedirectToAction("Details", "Groups", new { id = model.GroupId });
        }
    }
}
