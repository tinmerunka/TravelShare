namespace TravelShare.Models.Groups
{
    public class GroupMember
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Role { get; set; } = "Member";
        // "Member", "Moderator", "Admin"

        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}
