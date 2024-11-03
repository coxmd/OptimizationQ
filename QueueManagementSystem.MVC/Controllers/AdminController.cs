using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementSystem.MVC.Models.Privileges;

namespace Queue_Management_System.Controllers
{
    
    public class AdminController : Controller
    {
        [HttpGet]
        [HasPrivilege(PrivilegeConstants.AccessDashboard)]
        public IActionResult Dashboard()
        {
            ViewData["BreadcrumbTitle"] = "Dashboard";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageServices)]
        public IActionResult ConfigureServices()
        {
            ViewData["BreadcrumbTitle"] = "Configure Services";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageServiceCategories)]
        public IActionResult ConfigureServiceCategories()
        {
            ViewData["BreadcrumbTitle"] = "Configure Service Categories";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageServicePoints)]
        public IActionResult ConfigureServicePoints()
        {
            ViewData["BreadcrumbTitle"] = "Configure Service Points";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageServiceProviders)]
        public IActionResult ConfigureServiceProviders()
        {
            ViewData["BreadcrumbTitle"] = "Configure Service Providers";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageAdverts)]
        public IActionResult ManageAdverts()
        {
            ViewData["BreadcrumbTitle"] = "Manage Adverts";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ViewAnalytics)]
        public IActionResult Analytics()
        {
            ViewData["BreadcrumbTitle"] = "Analytics";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageReports)]
        public IActionResult ReportType()
        {
            ViewData["BreadcrumbTitle"] = "Report Type";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ConfigureSmtp)]
        public IActionResult ConfigureSmtp()
        {
            ViewData["BreadcrumbTitle"] = "Configure Smtp";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageCompanyInfo)]
        public IActionResult CompanyInformation()
        {
            ViewData["BreadcrumbTitle"] = "Company Information";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ConfigureSms)]
        public IActionResult ConfigureSms()
        {
            ViewData["BreadcrumbTitle"] = "Configure Sms";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManageUserRoles)]
        public IActionResult UserRole()
        {
            ViewData["BreadcrumbTitle"] = "User Roles";
            return View();
        }

        [HasPrivilege(PrivilegeConstants.ManagePrivileges)]
        public IActionResult Privilege()
        {
            ViewData["BreadcrumbTitle"] = "Privilege";
            return View();
        }
        
    }
}