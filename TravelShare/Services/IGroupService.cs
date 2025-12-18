using System.Collections.Generic;
using System.Threading.Tasks;
using TravelShare.Models.Groups;
using TravelShare.ViewModels;
using TravelShare.ViewModels.SocialGroups;


namespace TravelShare.Services.Groups
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(Group group, int creatorUserId);
        Task<Group> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<Group>> GetUserGroupsAsync(int userId);

        Task<bool> AddGroupMemberAsync(int groupId, int userId);
        Task<bool> RemoveGroupMemberAsync(int groupId, int userId);

        Task<GroupMessage> SendMessageAsync(GroupMessage message);
        Task<GroupPost> CreatePostAsync(GroupPost post);
        Task<IEnumerable<GroupMessage>> GetMessagesAsync(int groupId);
        Task<IEnumerable<GroupPost>> GetPostsAsync(int groupId);
        string? GetAllGroups();
        string? GetGroupById(int id);
        void CreateGroup(GroupViewModel model);
        void SendMessage(GroupMessageViewModel model);
        void AddExpense(GroupViewModel model);
    }
}
