namespace QueueManagementSystem.MVC.Services
{
    public class QueueCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QueueCleanupBackgroundService> _logger;

        public QueueCleanupBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<QueueCleanupBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Queue cleanup service running at: {time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                        await ticketService.CleanupExpiredTickets();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while cleaning up the queue");
                }

                // Wait for an hour before the next cleanup
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
