namespace QueueManagementSystem.MVC.Models;
using System.ComponentModel.DataAnnotations;

public class ServiceProvider
{
    public int Id {get; set;}

    [Required]
    public string? FullNames {get; set;}

    [Required]
    [EmailAddress]
    public string? Email {get; set;}

    [Required]
    public int ServiceId { get; set; }  // Changed from ServicePointId

    public Service? Service { get; set; }

    //TODO: reg expression for password requirements
    [Required]
    [StringLength(100, ErrorMessage = "Password must be atleast 8 characters long", MinimumLength = 8)]
    public string? Password {get; set;}

    [Required]
    public bool IsAdmin {get; set;}
    public bool Active { get; set; } 

}