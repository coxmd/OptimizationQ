using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QueueManagementSystem.MVC.Models.Privileges
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HasPrivilegeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _privilege;

        public HasPrivilegeAttribute(string privilege)
        {
            _privilege = privilege;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity!.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (!user.HasClaim(c => c.Type == "Privilege" && c.Value == _privilege) &&
                !user.IsInRole("Admin")) // Admin bypass
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
