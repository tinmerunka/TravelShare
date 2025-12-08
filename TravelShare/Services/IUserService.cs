using TravelShare.Models.Users;

namespace TravelShare.Services;
public interface IUserService
{
    Task<UserBase?> GetUserByIdAsync(int userId);
    Task<UserBase?> GetUserByEmailAsync(string email);
    Task<bool> UpdateUserProfileAsync(UserBase user);
    Task<IEnumerable<UserBase>> GetAllUsersAsync();
    Task<bool> ResetPasswordAsync(string email);
    Task<bool> UpdateTravelPreferencesAsync(int userId, TravelPreferences preferences);
}
