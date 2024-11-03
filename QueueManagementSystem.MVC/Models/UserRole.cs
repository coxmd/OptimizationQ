using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public List<RolePrivilege> RolePrivileges { get; set; }

    }
}
