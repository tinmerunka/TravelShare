namespace TravelShare.Models.Trips
{
    public class Trip
    {
        public int TripId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Destination { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreatedByUserId { get; set; }

        public bool IsPrivate { get; set; }

        public List<TripMember> Members { get; set; } = new List<TripMember>();

        public List<TripMedia> Media { get; set; } = new List<TripMedia>();

        public List<TripInvitation> Invitations { get; set; } = new List<TripInvitation>();
        public bool IsArchived { get; internal set; }
    }
}
