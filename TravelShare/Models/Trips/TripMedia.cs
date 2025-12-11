namespace TravelShare.Models.Trips
{
    public class TripMedia
    {
        public int Id { get; set; }

        public int TripId { get; set; }

        public string MediaUrl { get; set; }

        public string MediaType { get; set; }

        public int UploadedByUserId { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
