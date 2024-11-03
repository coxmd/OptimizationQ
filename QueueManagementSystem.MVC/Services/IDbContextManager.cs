using QueueManagementSystem.MVC.Data;

namespace QueueManagementSystem.MVC.Services
{
    public interface IDbContextManager
    {
        Task<T> ExecuteInContextAsync<T>(Func<QueueManagementSystemContext, Task<T>> operation);
        Task ExecuteInContextAsync(Func<QueueManagementSystemContext, Task> operation);
    }
}
