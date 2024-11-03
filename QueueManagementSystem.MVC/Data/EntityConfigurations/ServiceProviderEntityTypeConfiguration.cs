using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceProvider = QueueManagementSystem.MVC.Models.ServiceProvider;

namespace QueueManagementSystem.MVC.Data.EntityConfigurations;

class ServiceProviderEntityTypeConfiguration : IEntityTypeConfiguration<ServiceProvider>
{
    public void Configure(EntityTypeBuilder<ServiceProvider> builder)
    {
        builder.ToTable("ServiceProvider");

        //builder.Ignore(sp => sp.ConfirmPassword);

        builder.Property(sp => sp.Id).ValueGeneratedOnAdd();

        builder.HasOne(sp => sp.Service).WithMany().HasForeignKey(sp => sp.ServiceId).OnDelete(DeleteBehavior.Restrict);

    }
}