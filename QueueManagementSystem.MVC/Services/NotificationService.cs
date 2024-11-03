using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public class NotificationService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;
        public NotificationService(IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<List<NotificationMessage>> FetchNotifications()
        {
            var context = _dbFactory.CreateDbContext();
            var messageList = await context.Notifications.OrderBy(c => c.Date).ToListAsync();
            return messageList;

        }
        public async Task<bool> MarkNotificationAsRead(string TicketNo)
        {
            var context = _dbFactory.CreateDbContext();
            var message = await context.Notifications.FirstOrDefaultAsync(c =>c.TicketNo == TicketNo);
            if (message != null)
            {
                message.IsRead = true;
                context.Update(message);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

            }
        }
}
