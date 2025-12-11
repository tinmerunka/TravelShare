using System.Collections.Generic;

namespace TravelShare.ViewModels.SocialGroups
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }

        public IEnumerable<GroupMemberViewModel> Members { get; set; }
        public IEnumerable<GroupMessageViewModel> Messages { get; set; }
        public IEnumerable<GroupPostViewModel> Posts { get; set; }
    }

    public class GroupPostViewModel
    {
    }

    public class GroupMessageViewModel
    {
        public int GroupId { get; internal set; }
    }

    public class GroupMemberViewModel
    {
    }
}

