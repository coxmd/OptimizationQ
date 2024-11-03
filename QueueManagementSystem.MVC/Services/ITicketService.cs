using System;
using System.Speech.Synthesis;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public interface ITicketService
    {
        Task<Ticket> GenerateTicketAsync(string serviceName, string customerName,
        string customerPhoneNumber, string IdNumber, bool isEmergency = false, bool isLocked = false);

        Task TransferTicket(int ticketId, string newServiceName);

        Task MarkTicketAsEmergencyAsync(int ticketId);
        Task RedistributeTicketsAsync(string serviceName);
        Task<Ticket?> GetTicketFromQueueAsync(string serviceName, string calledServicePointName);
        Task UpdateTicketNoShowStatusAsync(int ticketId, bool wasNoShow, DateTime? noShowTime);
        Task MoveTicketToEndOfQueueAsync(int ticketId);
        Task MoveTicketToEndOfServicePointQueueAsync(int ticketId, int servicePointId);
        Task<bool> HasActiveTicketAsync(int servicePointId, string? serviceProviderId = null, string? excludeTicketNumber = null);
        Task CallTicketAsync(string ticketNumber, string calledServicePointName, int servicePointId, string? serviceProviderId = null);
        //Task CallTicketAsync(string ticketNumber, string calledServicePointName);

        Ticket? GetNoShowTicket(string serviceName, string calledServicePointName);

        Task<List<Ticket>> GetTicketsByServiceNameAsync(string serviceName);
        Task<bool> LockTicketAsync(int ticketId, string currentServiceProviderId);
        Task UnlockTicketAsync(int ticketId);
        Task<bool> IsPoolingEnabledAsync();

        Task<List<Ticket>> GetTicketsByServicePointIdAsync(int servicePointId);
        Task<List<Ticket>> GetQueuedTicketsForServicePointAsync(int servicePointId);
        Task<ServicePoint> GetCurrentServicePointAsync();
        Task<List<Ticket>> GetAllQueuedTicketsAsync();
        Task<int> GetCustomersAheadCountAsync(int ticketId);
        Task<List<Service>> GetAvailableServicesAsync();
        Task<TimeSpan> EstimateWaitingTimeAsync(int ticketId);
        Task<TimeSpan> GetAverageServiceTimeAsync(int servicePointId);
        Task UpdateTicketStatusAsync(int id, string status);
        Task UpdateTicketStatusAsync(int id, string status, DateTime showUpTime);

        Task UpdateServicePointStatusAsync(int servicePointId, string status);

        Task RemoveTicketFromQueueAsync(Ticket ticket);

        Task SaveServedTicketAsync(ServedTicket servedTicket);
        Task CleanupExpiredTickets();


        // Declare the event.
        public event EventHandler TicketQueueAlteredEvent;

        public event EventHandler<(string, string)> TicketCalledFromQueueEvent;

        public event EventHandler<string> TicketRemovedFromCalledEvent;
    }
}
