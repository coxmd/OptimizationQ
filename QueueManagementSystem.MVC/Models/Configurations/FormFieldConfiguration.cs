namespace QueueManagementSystem.MVC.Models.Configurations
{
    public class FormFieldConfiguration
    {
        public bool ShowIdNumber { get; set; } = true;
        public bool RequireIdNumber { get; set; } = true;

        public bool ShowName { get; set; } = true;
        public bool RequireName { get; set; } = true;

        public bool ShowPhoneNumber { get; set; } = true;
        public bool RequirePhoneNumber { get; set; } = true;

        public bool ShowEmail { get; set; } = false;
        public bool RequireEmail { get; set; } = false;

    }
}
