using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelShare.Controllers;
using TravelShare.Models.Trips;
using TravelShare.ViewModels.Trips;

namespace TravelShare.Services.Trips
{
    public class TripService : ITripService
    {
        private readonly List<Trip> _trips = new();
        private readonly List<TripInvitation> _invitations = new();

        public Task<Trip> CreateTripAsync(Trip trip, int creatorUserId)
        {
            trip.CreatedByUserId = creatorUserId;
            trip.TripId = _trips.Count + 1;
            _trips.Add(trip);

            return Task.FromResult(trip);
        }

        public Task<Trip> GetTripByIdAsync(int tripId)
        {
            return Task.FromResult(_trips.FirstOrDefault(t => t.TripId == tripId));
        }

        public Task<IEnumerable<Trip>> GetUserTripsAsync(int userId)
        {
            var result = _trips.Where(t => t.Members.Any(m => m.UserId == userId));
            return Task.FromResult(result);
        }

        public Task<bool> AddTripMemberAsync(int tripId, int userId)
        {
            var trip = _trips.FirstOrDefault(t => t.TripId == tripId);
            if (trip == null) return Task.FromResult(false);

            trip.Members.Add(new TripMember { TripId = tripId, UserId = userId });
            return Task.FromResult(true);
        }

        public Task<bool> RemoveTripMemberAsync(int tripId, int userId)
        {
            var trip = _trips.FirstOrDefault(t => t.TripId == tripId);
            if (trip == null) return Task.FromResult(false);

            var member = trip.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null) return Task.FromResult(false);

            trip.Members.Remove(member);
            return Task.FromResult(true);
        }

        public Task<TripInvitation> InviteUserAsync(int tripId, int inviterId)
        {
            var invitation = new TripInvitation
            {
                Id = _invitations.Count + 1,
                TripId = tripId,
                InvitedUserId = inviterId,                
                Status = "Pending"
            };

            _invitations.Add(invitation);
            return Task.FromResult(invitation);
        }

        public Task<bool> RespondToInvitationAsync(int invitationId, bool accept)
        {
            var inv = _invitations.FirstOrDefault(i => i.Id == invitationId);
            if (inv == null) return Task.FromResult(false);

            inv.Status = accept ? "Accepted" : "Rejected";
            return Task.FromResult(true);
        }

        public Task<bool> AddMediaAsync(int tripId, TripMedia media)
        {
            var trip = _trips.FirstOrDefault(t => t.TripId == tripId);
            if (trip == null) return Task.FromResult(false);

            trip.Media.Add(media);
            return Task.FromResult(true);
        }

        public Task<bool> ArchiveTripAsync(int tripId)
        {
            var trip = _trips.FirstOrDefault(t => t.TripId == tripId);
            if (trip == null) return Task.FromResult(false);

            trip.IsArchived = true;
            return Task.FromResult(true);
        }

        public Task<TripInvitation> InviteUserAsync(int tripId, int inviterId, string email)
        {
            throw new NotImplementedException();
        }

        string? ITripService.GetAllTrips()
        {
            throw new NotImplementedException();
        }

        string? ITripService.GetTripById(int id)
        {
            throw new NotImplementedException();
        }

        void ITripService.CreateTrip(TripViewModel model)
        {
            throw new NotImplementedException();
        }

        string? ITripService.GetTripForEdit(int id)
        {
            throw new NotImplementedException();
        }

        void ITripService.UpdateTrip(TripViewModel model)
        {
            throw new NotImplementedException();
        }

        void ITripService.DeleteTrip(int id)
        {
            throw new NotImplementedException();
        }

        void ITripService.AddMember(int tripId, int userId)
        {
            throw new NotImplementedException();
        }

        void ITripService.RemoveMember(int tripId, int userId)
        {
            throw new NotImplementedException();
        }

        void ITripService.AddMedia(TripMediaViewModel model)
        {
            throw new NotImplementedException();
        }

        void ITripService.DeleteMedia(int id)
        {
            throw new NotImplementedException();
        }

        void ITripService.SendInvitation(int tripId, int userId)
        {
            throw new NotImplementedException();
        }

        void ITripService.AcceptInvitation(int invitationId)
        {
            throw new NotImplementedException();
        }

        void ITripService.DeclineInvitation(int invitationId)
        {
            throw new NotImplementedException();
        }

        void ITripService.UpdateTrip(TripEditViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}

