

namespace QueueManagementSystem.MVC.Models
{
    public class ServiceStat
    {
        public string Name {get; set;}

        public TimeSpan AverageWaitingTime {get; set;}

        public TimeSpan AverageServiceTime {get; set;}

        public int TotalOfferings {get; set;}
    }
}