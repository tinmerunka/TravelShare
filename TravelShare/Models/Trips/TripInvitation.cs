namespace TravelShare.Models.Trips
{
    public class TripInvitation
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public int InvitedUserId { get; set; }

        public int SentByUserId { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Pending / Accepted / Declined
    }
}
