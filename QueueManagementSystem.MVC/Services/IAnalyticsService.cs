using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public interface IAnalyticsService
    {
        Task<List<ServiceStat>> GetServicesStatsAsync();

        Task<List<ServiceStat>> GetServicesStatsAsync(DateTime date);

        Task<List<ServiceStat>> GetServicesStatsAsync(DateTime startDate, DateTime endDate);

        Task<int> GetTotalServedTicketsAsync();

        Task<int> GetTotalServedTicketsAsync(DateTime date);

        Task<int> GetTotalServedTicketsAsync(DateTime startDate, DateTime endDate);

        //Task<ServiceStat?> GetServiceStatByNameAsync(string serviceName);
    }

}