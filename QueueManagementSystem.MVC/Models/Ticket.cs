

using Humanizer;

namespace QueueManagementSystem.MVC.Models
{
    public class Ticket
    {
        public int Id{get; set;}
        public string TicketNumber{get; set;}
        public string ServiceName {get; set;}
        public DateTime PrintTime {get; set;}
        public DateTime ServicePointAssignmentTime { get; set; }
        public DateTime ShowUpTime { get; set; }
        public string Status {get; set;}
        public string? CustomerName { get; set; } = null;
        public string? CustomerPhoneNumber { get; set; } = null;
        public bool? IsEmergency { get; set; }
        public string? IdNumber { get; set; } = null;
        public bool IsLocked { get; set; }
        public string? LockedByServiceProviderId { get; set; }
        public bool WasNoShow { get; set; } = false;
        public DateTime? LastNoShowTime { get; set; }

        // Foreign key to ServicePoint
        public int ServicePointId { get; set; }
        public ServicePoint? ServicePoint { get; set; }

        // Navigation property for Biometrics
        public int? BiometricsId { get; set; }
        public Biometrics? Biometrics { get; set; }
    }
}