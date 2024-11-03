using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Components;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models.Users;

namespace QueueManagementSystem.MVC.Services
{
    public class SystemUsersService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _db;

        public SystemUsersService(IDbContextFactory<QueueManagementSystemContext> db)
        {
            _db = db;
        }
        public async Task<bool> AddUser(SystemUsersModel usersModel)
        {
            try
            {
                using var context = _db.CreateDbContext();
                context.SystemUsers.Add(usersModel);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }




        }
        public async Task<bool> EditUser(SystemUsersModel usersModel)
        {
            try
            {
                using var context = _db.CreateDbContext();
                context.SystemUsers.Update(usersModel);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }
        }
        public async Task<List<SystemUsersModel>> FetUsers()
        {
            using var context = _db.CreateDbContext();
            return await context.SystemUsers.ToListAsync();
        }
        public async Task<SystemUsersModel?> FetUserName(string userName)
        {
            using var context = _db.CreateDbContext();
            
            return await context.SystemUsers.FirstOrDefaultAsync(h=>h.Username == userName);
        }
    }
}
