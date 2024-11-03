using System.Data;

namespace QueueManagementSystem.MVC.Models
{
    public class RolePrivilege
    {
        public int RoleId { get; set; }
        public UserRole Role { get; set; }

        public int PrivilegeId { get; set; }
        public Privilege Privilege { get; set; }
    }
}
