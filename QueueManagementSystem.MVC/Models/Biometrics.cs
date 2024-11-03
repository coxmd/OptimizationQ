using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class Biometrics
    {
        [Key]
        public int Id { get; set; }
        public string? IdNumber { get; set; }
        public byte[]? FingerPrintData { get; set; }
    }
}
