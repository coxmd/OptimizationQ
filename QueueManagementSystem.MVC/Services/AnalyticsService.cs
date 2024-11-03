using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace QueueManagementSystem.MVC.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly QueueManagementSystemContext _context;

        public AnalyticsService(QueueManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceStat>> GetServicesStatsAsync()
        {
            var serviceStats =  await _context.ServedTickets.GroupBy(st => st.ServiceName)
                                .Select(g => new ServiceStat{
                                    Name = g.Key,
                                    AverageWaitingTime = TimeSpan.FromSeconds(g.Average(st => (st.ShowUpTime-st.ServicePointAssignmentTime).TotalSeconds)),
                                    AverageServiceTime = TimeSpan.FromSeconds(g.Average(st => (st.FinishTime-st.ShowUpTime).TotalSeconds)),
                                    TotalOfferings = g.Count()
                                }).ToListAsync();

            return serviceStats;
        }

        public async Task<List<ServiceStat>> GetServicesStatsAsync(DateTime date)
        {
            var serviceStats =  await _context.ServedTickets.Where(st => st.PrintTime.Date == date.Date).GroupBy(st => st.ServiceName)
                                .Select(g => new ServiceStat{
                                    Name = g.Key,
                                    AverageWaitingTime = TimeSpan.FromSeconds(g.Average(st => (st.ShowUpTime -st.ServicePointAssignmentTime).TotalSeconds)),
                                    AverageServiceTime = TimeSpan.FromSeconds(g.Average(st => (st.FinishTime-st.ShowUpTime).TotalSeconds)),
                                    TotalOfferings = g.Count()
                                }).ToListAsync();

            return serviceStats;
        }

        public async Task<List<ServiceStat>> GetServicesStatsAsync(DateTime startDate, DateTime endDate)
        {
            var serviceStats =  await _context.ServedTickets.Where(st => startDate.Date <= st.PrintTime.Date && st.PrintTime.Date <= endDate.Date).GroupBy(st => st.ServiceName)
                                .Select(g => new ServiceStat{
                                    Name = g.Key,
                                    AverageWaitingTime = TimeSpan.FromSeconds(g.Average(st => (st.ShowUpTime -st.ServicePointAssignmentTime).TotalSeconds)),
                                    AverageServiceTime = TimeSpan.FromSeconds(g.Average(st => (st.FinishTime-st.ShowUpTime).TotalSeconds)),
                                    TotalOfferings = g.Count()
                                }).ToListAsync();

            return serviceStats;
        }

        public async Task<int> GetTotalServedTicketsAsync()
        {
            var total = await _context.ServedTickets.CountAsync();
            return total;
        }

        public async Task<int> GetTotalServedTicketsAsync(DateTime date)
        {
            var total = await _context.ServedTickets.CountAsync(st => st.PrintTime.Date == date.Date);
            return total;
        }

        public async Task<int> GetTotalServedTicketsAsync(DateTime startDate, DateTime endDate)
        {
            var total = await _context.ServedTickets.CountAsync(st => startDate.Date <= st.PrintTime.Date && st.PrintTime.Date <= endDate.Date);
            return total;
        }

        //public async Task<ServiceStat?> GetServiceStatByNameAsync(string serviceName)
        //{
        //    var stat = await _context.ServedTickets.Where(st => st.ServiceName == serviceName).GroupBy(st => st.ServiceName)
        //                        .Select(g => new ServiceStat{
        //                          Name = g.Key,
        //                          AverageWaitingTime = TimeSpan.FromSeconds(g.Average(st => (st.ShowTime-st.PrintTime).TotalSeconds)),
        //                          AverageServiceTime = TimeSpan.FromSeconds(g.Average(st => (st.FinishTime-st.ShowTime).TotalSeconds)),
        //                          TotalOfferings = g.Count()
        //                      });
        //  
        //  return stat;
        //}
    }
}