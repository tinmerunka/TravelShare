using System.Collections.Generic;

namespace TravelShare.ViewModels.Trips
{
    public class TripViewModel
    {
        public int TripId { get; set; }
        public string Title { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }

        public IEnumerable<TripMemberViewModel> Members { get; set; }
        public IEnumerable<TripMediaViewModel> Media { get; set; }
        public object Id { get; internal set; }
    }

    public class TripMemberViewModel
    {
    }

    public class TripMediaViewModel
    {
        public int MediaId { get; internal set; }
        public object TripId { get; internal set; }
    }
}

