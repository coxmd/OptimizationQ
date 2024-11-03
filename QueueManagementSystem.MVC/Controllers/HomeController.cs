using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private readonly IMemcachedClient _memcachedClient;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["BreadcrumbTitle"] = "Login";
        return RedirectToAction("Login", "Account");
        //return View();
    }

    public IActionResult Feedback()
    {
        ViewData["BreadcrumbTitle"] = "Feedback";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
