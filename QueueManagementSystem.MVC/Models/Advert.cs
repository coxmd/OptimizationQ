using System.ComponentModel.DataAnnotations;

namespace QueueManagementSystem.MVC.Models
{
    public class Advert
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public int Duration { get; set; } // Duration in seconds

        public int Order { get; set; } // New property for ordering
    }
}
