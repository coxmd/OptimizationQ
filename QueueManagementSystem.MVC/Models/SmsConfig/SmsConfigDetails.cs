using QueueManagementSystem.MVC.Models.enums;
using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models.SmsConfig
{
    public class SmsConfigDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public ParameterType ParameterType { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public Classification Classification { get; set; }

        // Foreign key for SmsSettingsModel
        public int SmsSettingsModelId { get; set; }

        // Navigation property to SmsSettingsModel
        public SmsSettingsModel SmsSettingsModel { get; set; }

    }
}
