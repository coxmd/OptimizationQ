namespace QueueManagementSystem.MVC.Models
{
    public class ServiceProviderAssignment
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public int ServicePointId { get; set; }
        public DateTime AssignmentTime { get; set; }
        public DateTime? SignInTime { get; set; }
        public DateTime? SignOutTime { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }

        public ServiceProvider ServiceProvider { get; set; }
        public ServicePoint ServicePoint { get; set; }
    }
}
