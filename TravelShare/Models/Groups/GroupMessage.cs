namespace TravelShare.Models.Groups
{
    public class GroupMessage
    {
        public int MessageId { get; set; }

        public int GroupId { get; set; }

        public int SenderUserId { get; set; }

        public string MessageText { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
