using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QueueManagementSystem.MVC.Data.EntityConfigurations;

class ServicePointEntityTypeConfiguration : IEntityTypeConfiguration<ServicePoint>
{
    public void Configure(EntityTypeBuilder<ServicePoint> builder)
    {
        builder.ToTable("ServicePoint");

        builder.Property(sp => sp.Id).ValueGeneratedOnAdd();

        builder.HasOne(sp => sp.Service).WithMany();

    }
}