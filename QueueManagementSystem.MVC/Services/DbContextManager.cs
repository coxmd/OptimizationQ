using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;

namespace QueueManagementSystem.MVC.Services
{
    public class DbContextManager : IDbContextManager
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;

        public DbContextManager(IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<T> ExecuteInContextAsync<T>(Func<QueueManagementSystemContext, Task<T>> operation)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await operation(context);
        }

        public async Task ExecuteInContextAsync(Func<QueueManagementSystemContext, Task> operation)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            await operation(context);
        }
    }
}
