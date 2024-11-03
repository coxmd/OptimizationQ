namespace QueueManagementSystem.MVC.Services
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string to, string message);
    }
}
