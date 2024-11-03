using QueueManagementSystem.MVC.Models.enums;
using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models.Feedback
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public WaitTime WaitTime { get; set; }
        public WaitTimeAcceptance WaitTimeAcceptance { get; set; }
        public Challenges Challenges { get; set; }
        public string? ImprovementSuggestion { get; set; }
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}
