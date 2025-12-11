using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelShare.Models.Groups;
using TravelShare.Services.Groups;
using TravelShare.ViewModels;
using TravelShare.ViewModels.SocialGroups;

namespace TravelShare.Services.SocialGroups
{
    public class GroupService : IGroupService
    {
        private readonly List<Group> _groups = new();
        private readonly List<GroupMessage> _messages = new();
        private readonly List<GroupPost> _posts = new();

        public Task<Group> CreateGroupAsync(Group group, int creatorUserId)
        {
            group.GroupId = _groups.Count + 1;
            group.CreatedByUserId = creatorUserId;

            _groups.Add(group);
            return Task.FromResult(group);
        }

        public Task<Group> GetGroupByIdAsync(int groupId)
        {
            return Task.FromResult(_groups.FirstOrDefault(g => g.GroupId == groupId));
        }

        public Task<IEnumerable<Group>> GetUserGroupsAsync(int userId)
        {
            var result = _groups.Where(g =>
                g.Members.Any(m => m.UserId == userId));

            return Task.FromResult(result);
        }

        public Task<bool> AddGroupMemberAsync(int groupId, int userId)
        {
            var group = _groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null) return Task.FromResult(false);

            group.Members.Add(new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                Role = "Member"
            });

            return Task.FromResult(true);
        }

        public Task<bool> RemoveGroupMemberAsync(int groupId, int userId)
        {
            var group = _groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null) return Task.FromResult(false);

            var member = group.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null) return Task.FromResult(false);

            group.Members.Remove(member);
            return Task.FromResult(true);
        }

        public Task<GroupMessage> SendMessageAsync(GroupMessage message)
        {
            message.MessageId = _messages.Count + 1;
            _messages.Add(message);

            return Task.FromResult(message);
        }

        public Task<GroupPost> CreatePostAsync(GroupPost post)
        {
            post.PostId = _posts.Count + 1;
            _posts.Add(post);

            return Task.FromResult(post);
        }

        public Task<IEnumerable<GroupMessage>> GetMessagesAsync(int groupId)
        {
            var result = _messages.Where(m => m.GroupId == groupId);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<GroupPost>> GetPostsAsync(int groupId)
        {
            var result = _posts.Where(p => p.GroupId == groupId);
            return Task.FromResult(result);
        }

        public string? GetAllGroups()
        {
            throw new NotImplementedException();
        }

        public string? GetGroupById(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateGroup(GroupViewModel model)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(GroupMessageViewModel model)
        {
            throw new NotImplementedException();
        }

        public void AddExpense(GroupViewModel model)
        {
            throw new NotImplementedException();
        }

    }
}

