using Microsoft.AspNetCore.Mvc;

namespace QueueManagementSystem.MVC.Controllers
{
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public IActionResult ConfigurationPage()
        {
            return View();
        }
    }
}
