using System.Collections.Generic;
using System.Threading.Tasks;
using TravelShare.Controllers;
using TravelShare.Models.Trips;
using TravelShare.ViewModels.Trips;

namespace TravelShare.Services.Trips
{
    public interface ITripService
    {
        Task<Trip> CreateTripAsync(Trip trip, int creatorUserId);
        Task<Trip> GetTripByIdAsync(int tripId);
        Task<IEnumerable<Trip>> GetUserTripsAsync(int userId);
        Task<bool> AddTripMemberAsync(int tripId, int userId);
        Task<bool> RemoveTripMemberAsync(int tripId, int userId);

        Task<TripInvitation> InviteUserAsync(int tripId, int inviterId, string email);
        Task<bool> RespondToInvitationAsync(int invitationId, bool accept);

        Task<bool> AddMediaAsync(int tripId, TripMedia media);
        Task<bool> ArchiveTripAsync(int tripId);
        string? GetAllTrips();
        string? GetTripById(int id);
        void CreateTrip(TripViewModel model);
        string? GetTripForEdit(int id);
        void UpdateTrip(TripEditViewModel model);
        void DeleteTrip(int id);
        void AddMember(int tripId, int userId);
        void RemoveMember(int tripId, int userId);
        void AddMedia(TripMediaViewModel model);
        void DeleteMedia(int id);
        void SendInvitation(int tripId, int userId);
        void AcceptInvitation(int invitationId);
        void DeclineInvitation(int invitationId);
        void UpdateTrip(TripViewModel model);
    }
}
