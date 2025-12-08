using TravelShare.Models.Users;

namespace TravelShare.ViewModels;
public class UserProfileViewModel
{
    public UserBase User { get; set; } = null!;
    public Student? StudentProfile { get; set; }
    public Administrator? AdminProfile { get; set; }
}
