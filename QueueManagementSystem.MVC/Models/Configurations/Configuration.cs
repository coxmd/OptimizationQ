namespace QueueManagementSystem.MVC.Models.Configurations
{
    public class Configuration
    {
        public int Id { get; set; }
        public string ConfigurationName { get; set; }

        // Properties to store different types of configuration values
        public string StringValue { get; set; }
        public int? IntValue { get; set; }
        public bool? BoolValue { get; set; }

        // Enum to indicate the type of value stored
        public ConfigurationType ValueType { get; set; }

        public enum ConfigurationType
        {
            String = 1,
            Integer = 2,
            Boolean = 3,
        }
    }
}
