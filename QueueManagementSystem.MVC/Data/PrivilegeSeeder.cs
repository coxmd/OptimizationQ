using QueueManagementSystem.MVC.Models.Privileges;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Data
{
    public class PrivilegeSeeder
    {
        public static void SeedPrivileges(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<QueueManagementSystemContext>();

                // Add all privileges
                var privileges = new List<Privilege>
                {
                    // Admin Dashboard Privileges
                    new Privilege { Name = PrivilegeConstants.AccessDashboard },
                    new Privilege { Name = PrivilegeConstants.ManageServices },
                    new Privilege { Name = PrivilegeConstants.ManageServiceCategories },
                    new Privilege { Name = PrivilegeConstants.ManageServicePoints },
                    new Privilege { Name = PrivilegeConstants.ManageServiceProviders },
                    new Privilege { Name = PrivilegeConstants.ManageAdverts },
                    new Privilege { Name = PrivilegeConstants.ViewAnalytics },
                    new Privilege { Name = PrivilegeConstants.ManageReports},
                    new Privilege { Name = PrivilegeConstants.ConfigureSmtp },
                    new Privilege { Name = PrivilegeConstants.ManageCompanyInfo },
                    new Privilege { Name = PrivilegeConstants.ConfigureSms},
                    new Privilege { Name = PrivilegeConstants.ManageUserRoles },
                    new Privilege { Name = PrivilegeConstants.ManagePrivileges },

                    // Queue Management Privileges
                    new Privilege { Name = PrivilegeConstants.AccessServicePoint},
                    new Privilege { Name = PrivilegeConstants.ViewMainQueue },
                
                };

                foreach (var privilege in privileges)
                {
                    if (!context.Privileges.Any(p => p.Name == privilege.Name))
                    {
                        context.Privileges.Add(privilege);
                    }
                }

                // Create Admin role if it doesn't exist
                var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
                if (adminRole == null)
                {
                    adminRole = new UserRole { Name = "Admin" };
                    context.Roles.Add(adminRole);
                    context.SaveChanges();

                    // Assign all privileges to Admin role
                    foreach (var privilege in context.Privileges)
                    {
                        context.RolePrivileges.Add(new RolePrivilege
                        {
                            RoleId = adminRole.Id,
                            PrivilegeId = privilege.Id
                        });
                    }
                }

                // Create Service Provider role if it doesn't exist
                var serviceProviderRole = context.Roles.FirstOrDefault(r => r.Name == "Service Provider");
                if (serviceProviderRole == null)
                {
                    serviceProviderRole = new UserRole { Name = "Service Provider" };
                    context.Roles.Add(serviceProviderRole);
                    context.SaveChanges();

                    // Get Service Provider specific privileges
                    var serviceProviderPrivileges = context.Privileges.Where(p =>
                        p.Name == PrivilegeConstants.AccessServicePoint ||
                        p.Name == PrivilegeConstants.ViewMainQueue
                    ).ToList();

                    // Assign specific privileges to Service Provider role
                    foreach (var privilege in serviceProviderPrivileges)
                    {
                        context.RolePrivileges.Add(new RolePrivilege
                        {
                            RoleId = serviceProviderRole.Id,
                            PrivilegeId = privilege.Id
                        });
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
