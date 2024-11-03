using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementSystem.MVC.Models.Privileges;
using QueueManagementSystem.MVC.Services;

namespace Queue_Management_System.Controllers
{
    public class QueueController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public QueueController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        [Route("Queue/CheckinPage")]
        [Route("Queue/services-mobile")]
        public async Task<IActionResult> CheckinPage()
        {
            return View();
        }

        [HttpGet]
        [Route("Queue/QRCodePage")]
        public async Task<IActionResult> QRCodePage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult WaitingPage()
        {
            return View();
        }

        [HasPrivilege(PrivilegeConstants.AccessServicePoint)]
        [Authorize, HttpGet]
        public IActionResult ServicePoint()
        {
            ViewData["BreadcrumbTitle"] = "Service Point";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ViewMainQueue)]
        [HttpGet]
        public IActionResult MainQueue()
        {
            ViewData["BreadcrumbTitle"] = "Main Queue";
            return View();
        }

        [HttpGet]
        [Route("Queue/TicketTrackingPage/{ticketNumber}")]
        public IActionResult TicketTrackingPage(string ticketNumber)
        {
            return View(model: ticketNumber);
        }
    }
}