using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class ChangePasswordModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserEmail { get; set; }
        [Required]
        public string? OldPassword { get; set; }
        [Required]
        public string? NewPassword { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
