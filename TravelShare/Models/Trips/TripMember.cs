namespace TravelShare.Models.Trips
{
    public class TripMember
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public int UserId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public string Role { get; set; } = "Member";
    }
}
