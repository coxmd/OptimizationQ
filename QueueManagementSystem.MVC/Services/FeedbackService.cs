using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models.Feedback;

namespace QueueManagementSystem.MVC.Services
{
    public class FeedbackService
    {
        private readonly IDbContextFactory<QueueManagementSystemContext> _db;
        public FeedbackService(IDbContextFactory<QueueManagementSystemContext> db)
        {
                _db = db;
        }
        public async Task SaveFeedbackAsync(Feedback feedback)
        {
            using var context =  _db.CreateDbContext();
            context.Feedbacks.Add(feedback);
            await context.SaveChangesAsync();
        }

        public async Task<List<Feedback>> GetAllFeedbackAsync()
        {
            using var context = _db.CreateDbContext();
            return await context.Feedbacks.ToListAsync();
        }
    }
}
