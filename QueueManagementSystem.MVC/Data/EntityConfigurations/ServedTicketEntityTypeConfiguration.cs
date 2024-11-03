using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QueueManagementSystem.MVC.Data.EntityConfigurations;

class ServedTicketEntityTypeConfiguration : IEntityTypeConfiguration<ServedTicket>
{
    public void Configure(EntityTypeBuilder<ServedTicket> builder)
    {
        builder.ToTable("ServedTicket");

        builder.Property(st => st.Id).ValueGeneratedOnAdd();

        builder.Property(st => st.PrintTime).HasColumnType("timestamp");
        builder.Property(st => st.ShowUpTime).HasColumnType("timestamp");
        builder.Property(st => st.FinishTime).HasColumnType("timestamp");

    }
}