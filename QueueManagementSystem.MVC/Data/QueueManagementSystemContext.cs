using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Models.Smtp;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data.EntityConfigurations;
using ServiceProvider = QueueManagementSystem.MVC.Models.ServiceProvider;
using QueueManagementSystem.MVC.Models.SmsConfig;
using System.Data;
using System.Reflection.Emit;
using Humanizer;
using QueueManagementSystem.MVC.Models.Feedback;
using QueueManagementSystem.MVC.Models.Users;
using QueueManagementSystem.MVC.Models.Configurations;

namespace QueueManagementSystem.MVC.Data
{
    public class QueueManagementSystemContext : DbContext
    {
        public QueueManagementSystemContext(DbContextOptions<QueueManagementSystemContext> options) : base(options)
        {

        }

        public DbSet<Ticket> QueuedTickets {get; set;}
        public DbSet<Service> Services {get; set;}
        public DbSet<ServedTicket> ServedTickets {get; set;}
        public DbSet<ServiceProvider> ServiceProviders {get; set;}
        public DbSet<ServicePoint> ServicePoints {get; set;}
        public DbSet<Sms> SmsMessages { get; set; }
        public DbSet<EmailSettingsModel> SmptSettings { get; set; }
        public DbSet<NotificationMessage> Notifications { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Biometrics> Biodata { get; set; }
        
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<SmsSettingsModel> SmsSettings { get; set; }
        public DbSet<SmsConfigDetails> SmsConfigs { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<SystemUsersModel> SystemUsers { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<CompanyInformation> CompanyInformation { get; set; }
        public DbSet<ServiceProviderAssignment> ServiceProviderAssignments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Define one-to - many relationship
            builder.Entity<SmsConfigDetails>()
           .HasOne(s => s.SmsSettingsModel)
           .WithMany(d => d.SmsConfigDetails)
           .HasForeignKey(d => d.SmsSettingsModelId)
            .OnDelete(DeleteBehavior.Restrict);

                  // Define many-to-many relationship between Roles and Privileges
            builder.Entity<RolePrivilege>()
                .HasKey(rp => new { rp.RoleId, rp.PrivilegeId });
                

            builder.Entity<RolePrivilege>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePrivileges)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolePrivilege>()
                .HasOne(rp => rp.Privilege)
                .WithMany(p => p.RolePrivileges)
                .HasForeignKey(rp => rp.PrivilegeId)
                .OnDelete(DeleteBehavior.Restrict);


            // Configure the foreign key relationship between Service and ServiceCategory
            builder.Entity<Service>()
                .HasOne(s => s.ServiceCategory)
                .WithMany(sc => sc.Services)
                .HasForeignKey(s => s.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Optional: use Cascade or Restrict

            // Configure the Ticket and ServicePoint relationship
            builder.Entity<Ticket>()
                .HasOne(t => t.ServicePoint)
                .WithMany() 
                .HasForeignKey(t => t.ServicePointId)
                .OnDelete(DeleteBehavior.Restrict); // Optional, use Cascade or Restrict as needed

            //Configure the ServedTicket and ServicePoint relationship
            builder.Entity<ServedTicket>()
                .HasOne(t => t.ServicePoint)
                .WithMany()
                .HasForeignKey(t => t.ServicePointId)
                .OnDelete(DeleteBehavior.Restrict); // Optional, use Cascade or Restrict as needed

            // Configure the relationship between Ticket and Biometrics
            builder.Entity<Ticket>()
                .HasOne(t => t.Biometrics)
                .WithMany()
                .HasForeignKey(t => t.BiometricsId)
                .OnDelete(DeleteBehavior.Restrict); // Optional, use Cascade or Restrict as needed

            builder.ApplyConfiguration(new TicketEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServiceEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServedTicketEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServiceProviderEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServicePointEntityTypeConfiguration());

        }

        // Compiled queries for optimization
        public static readonly Func<QueueManagementSystemContext, string, Task<Service?>> GetServiceByNameAsync =
            EF.CompileAsyncQuery((QueueManagementSystemContext context, string serviceName) =>
                context.Services.FirstOrDefault(s => s.Name == serviceName));

        public static readonly Func<QueueManagementSystemContext, string, Task<ServicePoint?>> GetServicePointByNameAsync =
            EF.CompileAsyncQuery((QueueManagementSystemContext context, string servicePointName) =>
                context.ServicePoints.FirstOrDefault(sp => sp.Name == servicePointName));

        public static readonly Func<QueueManagementSystemContext, string, Task<ServiceCategory?>> GetServiceCategoryByNameAsync =
            EF.CompileAsyncQuery((QueueManagementSystemContext context, string serviceCategoryName) =>
                context.ServiceCategories.FirstOrDefault(sc => sc.Name == serviceCategoryName));

        public static readonly Func<QueueManagementSystemContext, string, Task<ServiceProvider?>> GetServiceProviderByEmailAsync =
            EF.CompileAsyncQuery((QueueManagementSystemContext context, string email) =>
                context.ServiceProviders.FirstOrDefault(sp => sp.Email == email));

        public static readonly Func<QueueManagementSystemContext, string, Task<Ticket?>> GetTicketByNumberAsync =
            EF.CompileAsyncQuery((QueueManagementSystemContext context, string ticketNumber) =>
                context.QueuedTickets.FirstOrDefault(t => t.TicketNumber == ticketNumber));

        public static readonly Func<QueueManagementSystemContext, string, Task<Configuration?>> GetConfigurationByNameAsync =
            EF.CompileAsyncQuery((QueueManagementSystemContext context, string configName) =>
                context.Configurations.FirstOrDefault(c => c.ConfigurationName == configName));
    }
}
