using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QueueManagementSystem.MVC.Data.EntityConfigurations;

class ServiceEntityTypeConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Service");

        builder.Property(s => s.Id).ValueGeneratedOnAdd();

    }
}