namespace TravelShare.Models.Groups
{
    public class GroupPost
    {
        public int PostId { get; set; }

        public int GroupId { get; set; }

        public int AuthorUserId { get; set; }

        public string Content { get; set; }

        public string MediaUrl { get; set; }  // optional slika/video uz post

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
