using TravelShare.ViewModels;
using TravelShare.Models.Users;

namespace TravelShare.Services.Factories;
public interface IUserFactory
{
    Student CreateStudent(RegisterViewModel model);
    Administrator CreateAdministrator(string email, string firstName, string lastName);
}