using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Models.Users;

namespace QueueManagementSystem.MVC.Services
{
    public interface IAuthService
    {
        Task<SystemUsersModel>? Authenticate(LoginModel user);
    }
}