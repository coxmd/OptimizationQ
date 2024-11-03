using QueueManagementSystem.MVC.Services;
using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class CustomerInfo
    {
        [RegularExpression(@"^[a-zA-Z\s]{2,50}$", ErrorMessage = "Name must be 2-50 characters long and contain only letters and spaces")]
        public string? Name { get; set; }
        [RegularExpression(@"^\d{1,12}$", ErrorMessage = "ID Number must be a valid Integer between 1 and 12 digits")]
        public string? Idnumber { get; set; }
        [RegularExpression(@"^\d{10,12}$", ErrorMessage = "Phone number must be a valid Integer between 10-12 digits")]
        public string? PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }
        public byte[]? FingerPrintData { get; set; }
    }
   
}
