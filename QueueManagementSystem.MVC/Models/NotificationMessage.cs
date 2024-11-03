using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class NotificationMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Message { get; set; }
        [Required]
        public string? TicketNo { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }
}
