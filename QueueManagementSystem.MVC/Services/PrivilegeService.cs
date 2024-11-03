using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Components;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Models.ViewModels;

namespace QueueManagementSystem.MVC.Services
{
    public class PrivilegeService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _db;
        public PrivilegeService(IDbContextFactory<QueueManagementSystemContext> db)
        {
            _db = db;
        }

        public async Task<List<PrivilegeViewModel>> GetAllPrivilegesAsync()
        {
            using var context = await _db.CreateDbContextAsync();
            return MapToPrivilegeViewModel(await context.Privileges.ToListAsync());
            
        }
        public async Task<bool> AddPrivilegeAsync(Privilege newPrivilege)
        {

            using var context = await _db.CreateDbContextAsync();
            // Check if the privilege already exists
            var existingPrivilege = await context.Privileges
                .FirstOrDefaultAsync(p => p.Name == newPrivilege.Name);

            if (existingPrivilege != null)
            {
                return false;
            }

            // Add the new privilege to the database
            context.Privileges.Add(newPrivilege);
            await context.SaveChangesAsync();
            return true;
        }
        public List<PrivilegeViewModel> MapToPrivilegeViewModel(List<Privilege> privileges)
        {
            return privileges.Select(p => new PrivilegeViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsSelected = false // Initially set to false, can be updated based on logic
            }).ToList();
        }
    }
}
