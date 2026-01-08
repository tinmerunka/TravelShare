using Microsoft.Extensions.Logging;
using TravelShare.Models.Users;
using TravelShare.Services.Interfaces;

namespace TravelShare.Services;
public class ProfileChangeLogger : IProfileUpdateObserver
{
    private readonly ILogger<ProfileChangeLogger> _logger;

    public ProfileChangeLogger(ILogger<ProfileChangeLogger> logger)
    {
        _logger = logger;
    }

    public Task OnProfileUpdatedAsync(User user)
    {
        _logger.LogInformation("Profile updated for user {Email} (Id: {Id})", user.Email, user.Id);
        return Task.CompletedTask;
    }
}