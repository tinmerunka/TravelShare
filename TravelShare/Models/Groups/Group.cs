namespace TravelShare.Models.Groups
{
    public class Group
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsPrivate { get; set; }

        public List<GroupMember> Members { get; set; } = new List<GroupMember>();

        public List<GroupPost> Posts { get; set; } = new List<GroupPost>();

        public List<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
    }
}
