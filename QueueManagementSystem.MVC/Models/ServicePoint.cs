namespace QueueManagementSystem.MVC.Models;
using System.ComponentModel.DataAnnotations;

public class ServicePoint
{
    public int Id {get; set;}

    [Required]
    public string? Name {get; set;}
    public string? Status {get; set;}

    [Required]
    public int ServiceId {get; set; }

    public Service? Service { get; set; }
    public bool Active { get; set; } = false;
}