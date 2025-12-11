using TravelShare.Models.Users;

namespace TravelShare.Services.Interfaces;
public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> UpdateUserProfileAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> ResetPasswordAsync(string email);
    Task<bool> UpdateTravelPreferencesAsync(int userId, TravelPreferences preferences);
}
