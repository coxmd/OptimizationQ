namespace QueueManagementSystem.MVC.Models
{
    public class FingerprintResult
    {
        public bool IsAuthenticated { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
    }
}
