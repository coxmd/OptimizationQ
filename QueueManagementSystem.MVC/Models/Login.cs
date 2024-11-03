using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        
        public string? UserName {get; set;}

        [Required(ErrorMessage = "Please enter a password")]
        public string? Password {get; set;}
    }
    
}