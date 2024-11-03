using QueueManagementSystem.MVC.Models.Smtp;

namespace QueueManagementSystem.MVC.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailMessageModel message);
    }
}
