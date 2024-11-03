using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Models.Users;

namespace QueueManagementSystem.MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly QueueManagementSystemContext _context;

        public AuthService(QueueManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<SystemUsersModel?> Authenticate(LoginModel user)
        {
            SystemUsersModel? provider = await _context.SystemUsers.SingleOrDefaultAsync(sp => sp.Username == user.UserName);

            //if (provider == null || (user.Password !=provider.Password))
            if (provider == null || !BCrypt.Net.BCrypt.Verify(user.Password, provider.Password))
            {
                return null;
            }

            return provider;
        }
    }
}