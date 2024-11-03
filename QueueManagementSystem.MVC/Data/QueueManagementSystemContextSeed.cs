using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore;


using ServiceProvider = QueueManagementSystem.MVC.Models.ServiceProvider;
using QueueManagementSystem.MVC.Services;

namespace QueueManagementSystem.MVC.Data
{
    public class QueueManagementSystemContextSeeder
    {
        public static void SeedServiceCategories(QueueManagementSystemContext context)
        {
            if (!context.ServiceCategories.Any())
            {
                var serviceCategories = new List<ServiceCategory>
            {
                new ServiceCategory { Id = 1, Name = "Medical Services", Description = "All health-related services provided by the hospital" },
                new ServiceCategory { Id = 2, Name = "Diagnostic Services", Description = "Laboratory and imaging services to diagnose conditions" },
                new ServiceCategory { Id = 3, Name = "Rehabilitation Services", Description = "Therapies to improve mobility and function" },
                new ServiceCategory { Id = 4, Name = "Preventive Services", Description = "Health maintenance and disease prevention services" },
            };

                context.AddRange(serviceCategories);
                context.SaveChanges();
            }
        }

        public static void SeedServices(QueueManagementSystemContext context)
        {
            if (!context.Services.Any())
            {
                var services = new List<Service>
                {
                    new Service { Id = 1, Name = "Dental Service", Description="Comprehensive dental care, from routine cleanings and exams to advanced procedures like implants and oral surgery.", ServiceCategoryId = 1 },
                    new Service { Id = 2, Name = "Diagnostic Service", Description="Comprehensive range of tests, from imaging (X-ray, CT, MRI) to lab work, to pinpoint the cause of your health concerns.", ServiceCategoryId = 2 },
                    new Service { Id = 3, Name = "Surgical Service", Description="Advanced techniques to perform a wide range of procedures, from minimally invasive to complex surgeries.", ServiceCategoryId = 1 },
                    new Service { Id = 4, Name = "Rehabilitation Service", Description="Regain strength, mobility, and independence with our hospital's rehabilitation services.", ServiceCategoryId = 3 },
                    new Service { Id = 5, Name = "Preventive Care Service", Description="Screenings, plans & guidance to help you prevent chronic diseases, catch issues early & live healthier.", ServiceCategoryId = 4 },
                };

                context.AddRange(services);
                context.SaveChanges();
            }
        }


        public static void SeedServicePoints(QueueManagementSystemContext context)
        {
            if (!context.ServicePoints.Any())
            {
                var servicePoints = new List<ServicePoint>
                {
                    new ServicePoint {Id = 1, Name = "ServicePoint 1", ServiceId = 1},
                    new ServicePoint {Id = 2, Name = "ServicePoint 2", ServiceId = 2},
                    new ServicePoint {Id = 3, Name = "ServicePoint 3", ServiceId = 2},
                    new ServicePoint {Id = 4, Name = "ServicePoint 4", ServiceId = 3},
                    new ServicePoint {Id = 5, Name = "ServicePoint 5", ServiceId = 4},
                    new ServicePoint {Id = 6, Name = "ServicePoint 6", ServiceId = 5},

                };

                context.AddRange(servicePoints);
                context.SaveChanges();

            }
        }

        //public static void SeedServiceProviders(QueueManagementSystemContext context)
        //{
        //    if (!context.ServiceProviders.Any())
        //    {
        //        var serviceProviders = new List<ServiceProvider>
        //        {
        //            new ServiceProvider {Id = 1, FullNames = "Daniel Mwambogha", Email = "dante@gmail.com", IsAdmin = true, Password = BCrypt.Net.BCrypt.HashPassword("Dante@09"), ServicePointId = 4},
        //            new ServiceProvider {Id = 2, FullNames = "Chris Omar", Email = "chris@gmail.com", IsAdmin = false, Password = BCrypt.Net.BCrypt.HashPassword("Chris@09"), ServicePointId = 2},
        //            new ServiceProvider {Id = 3, FullNames = "Amina Mohammed", Email = "amina@yahoo.com", IsAdmin = false, Password = BCrypt.Net.BCrypt.HashPassword("Amina@09"), ServicePointId = 3},
        //        };

        //        context.AddRange(serviceProviders);
        //        context.SaveChanges();

        //    }
        //}

        public static void ResetQueue(QueueManagementSystemContext context)
        {
            var queuedTickets = context.QueuedTickets
            .Select(ticket => new Ticket
            {
                Id = ticket.Id,
                CustomerName = ticket.CustomerName ?? string.Empty, // Handle null
                CustomerPhoneNumber = ticket.CustomerPhoneNumber ?? string.Empty,
                //CustomerEmail = ticket.CustomerEmail ?? string.Empty,
                PrintTime = ticket.PrintTime,
                ServiceName = ticket.ServiceName,
                Status = ticket.Status,
                TicketNumber = ticket.TicketNumber,
                ServicePointId = ticket.ServicePointId,
            }).ToList();
            // Materialize the query
            foreach (var ticket in queuedTickets)
            {
                ticket.Status = TicketStatus.InQueue;

                // Ensure customer information fields are not null
                ticket.CustomerName = ticket.CustomerName ?? string.Empty;
                ticket.CustomerPhoneNumber = ticket.CustomerPhoneNumber ?? string.Empty;
                //ticket.CustomerEmail = ticket.CustomerEmail ?? string.Empty;
            }
            context.UpdateRange(queuedTickets);
            context.SaveChanges();
        }
    }
}
