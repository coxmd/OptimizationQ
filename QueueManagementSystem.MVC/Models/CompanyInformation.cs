using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class CompanyInformation
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string PhysicalAddress { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? ReportFooterMessage { get; set; }

        public string? Logo { get; set; }  // Stored as base64 string

        public DateTime LastUpdated { get; set; }
    }
}
