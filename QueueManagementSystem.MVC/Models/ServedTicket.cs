using System;

namespace QueueManagementSystem.MVC.Models
{
    public class ServedTicket
    {
        public int Id {get; set;}
        public string TicketNumber{get; set;}
        public string ServiceName {get; set;}
        public string? CustomerName { get; set; } = null;
        public string? CustomerPhoneNumber { get; set; } = null;
        public string? IdNumber { get; set; } = null;
        public DateTime PrintTime {get; set;}
        public DateTime ServicePointAssignmentTime { get; set; }
        public DateTime ShowUpTime {get; set;}
        public DateTime FinishTime {get; set;}
        public int ServiceId { get; set;}
        //Foreign key to ServicePoint
        public int ServicePointId { get; set; }
        public ServicePoint? ServicePoint { get; set; }
    }
}