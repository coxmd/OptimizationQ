using System.Security.Claims;
using Enyim.Caching.Memcached;
using FastReport.AdvMatrix;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Components;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Models.Privileges;
using QueueManagementSystem.MVC.Models.Users;
using QueueManagementSystem.MVC.Services;

namespace Queue_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly PrivilegeService _privilegeService;
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;

        public AccountController(PrivilegeService privilegeService, IAuthService authService, IDbContextFactory<QueueManagementSystemContext> dbFactory)
        {
            _authService = authService;
            _privilegeService = privilegeService;
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(SystemUsersModel systemUsers)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel change)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using var context = _dbFactory.CreateDbContext();
            var user = context.ServiceProviders.FirstOrDefault(s => s.Email == change.UserEmail);
            if (user == null)
            {
                // Handle user not found case
                ModelState.AddModelError(string.Empty, "Email not Registered ");
                return View();
            }
            else
            {
                
                if (!BCrypt.Net.BCrypt.Verify(change.OldPassword, user.Password))
                {
                    
                    
                    ModelState.AddModelError(string.Empty, "Wrong Old password");
                    return View(change);

                }
                else
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);
                    context.Update(user);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "Your password has been changed successfully.";
                    return View();

                }

            }
            }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using var context = _dbFactory.CreateDbContext();
            var userName = context.SystemUsers.FirstOrDefault(s => s.Username == user.UserName);
             if(userName == null)
            {
                // Handle user not found case
                ModelState.AddModelError(string.Empty, "User not Registered ");
                return View();
            }

            var serviceProvider = await _authService.Authenticate(user)!;
            if (serviceProvider == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View();
            }
            var Rolename= context.Roles.FirstOrDefault(s => s.Id == serviceProvider.Role);

            //string? role = serviceProvider.IsAdmin ? "Admin" : "";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, serviceProvider.Id.ToString()),
                new Claim(ClaimTypes.Name, serviceProvider.Surname),
                new Claim(ClaimTypes.Role, Rolename.Name)
            };

            // Add privileges as claims

            var privileges = context.RolePrivileges.Where(p => p.RoleId == serviceProvider.Role).Select(p => p.Privilege).ToList();
            foreach (var privilege in privileges)                 
            {
                claims.Add(new Claim("Privilege", privilege.Name));
                
            }

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieScheme");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync("MyCookieScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect based on role
            return Rolename.Name switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Service Provider" => RedirectToAction("ServicePoint", "Queue"),
                _ => RedirectToAction("ServicePoint", "Queue") // Default to ServicePoint if role is unknown
            };

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieScheme");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}