using BlazorBootstrap;
using FastReport.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QueueManagementSystem.MVC.Components;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Models.ViewModels;
using System.Data;

namespace QueueManagementSystem.MVC.Services
{
    public class RoleService
    {

        private readonly IDbContextFactory<QueueManagementSystemContext> _db;
        private readonly PrivilegeService _privilegeService;
        public RoleService(IDbContextFactory<QueueManagementSystemContext> db, PrivilegeService privilegeService)
        {
            _db = db;
            _privilegeService = privilegeService;
        }
        public async Task<List<UserRole>> GetRolesAsync()
        {
            using var context = await _db.CreateDbContextAsync();
            List<UserRole> userRoles = await context.Roles.ToListAsync();
            return userRoles;
        }

        // Get privileges assigned to a specific role
        public async Task<List<PrivilegeViewModel>> GetPrivilegesForRoleAsync(int roleId)
        {

            using var context = await _db.CreateDbContextAsync();
            // Get only the privileges that are assigned to the role
            var assignedPrivileges = await context.Privileges
                .Where(p => context.RolePrivileges
                    .Where(rp => rp.RoleId == roleId)
                    .Select(rp => rp.PrivilegeId)
                    .Contains(p.Id))
                .ToListAsync();

            // Map only the assigned privileges to ViewModel
            var privilegeViewModels = _privilegeService.MapToPrivilegeViewModel(assignedPrivileges);

            return privilegeViewModels;
            
        }
     

        // Add a privilege to a role
        public async Task AddPrivilegeToRoleAsync(int roleId, int privilegeId)
        {
            using var context = await _db.CreateDbContextAsync();
            var rolePrivilege = new RolePrivilege
            {
                RoleId = roleId,
                PrivilegeId = privilegeId
            };

            context.RolePrivileges.Add(rolePrivilege);
            await context.SaveChangesAsync();
        }

        // Remove a privilege from a role
        public async Task RemovePrivilegeFromRoleAsync(int roleId, int privilegeId)
        {
            try
            {
                using var context = await _db.CreateDbContextAsync();
                var rolePrivilege = await context.RolePrivileges.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PrivilegeId == privilegeId);
                if (rolePrivilege != null)
                {
                    context.RolePrivileges.Remove(rolePrivilege);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //ToastService.Notify(new(ToastType.Danger, $"Error: {ex.Message}."));
            }

        }
    }
}
