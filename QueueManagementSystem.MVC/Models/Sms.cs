using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class Sms
    {
        [Key]
        public int Id { get; set; }
        public string? MobileNo { get; set; }
        public string? Message { get; set; }
        public bool SentStatus { get; set; }=false;
    }
}
