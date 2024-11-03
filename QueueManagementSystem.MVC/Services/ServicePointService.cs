using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public class ServicePointService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;

        public ServicePointService(IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<ServicePoint> GetServicePointAsync(int servicePointId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.ServicePoints
                .Include(sp => sp.Service)
                .FirstOrDefaultAsync(sp => sp.Id == servicePointId);
        }

        public async Task<string> GetServicePointStatusAsync(int servicePointId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var servicePoint = await context.ServicePoints.FindAsync(servicePointId);
            return servicePoint?.Status ?? "Available";
        }

        public async Task UpdateServicePointStatusAsync(int servicePointId, string status)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var servicePoint = await context.ServicePoints.FindAsync(servicePointId);
            if (servicePoint != null)
            {
                servicePoint.Status = status;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Services.ToListAsync();
        }
    }
}