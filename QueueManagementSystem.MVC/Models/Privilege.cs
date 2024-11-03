using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class Privilege
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        
        public List<RolePrivilege> RolePrivileges { get; set; }

    }
}
