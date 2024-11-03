using QueueManagementSystem.MVC.Models.enums;
using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models.SmsConfig
{
    public class SmsSettingsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Url { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public MethodType HttpMethod { get; set; }

        [Required]
        public ResponseType ResponseType { get; set; }
        // Navigation property for the one-to-many relationship
        public ICollection<SmsConfigDetails>? SmsConfigDetails { get; set; }
    }
}
