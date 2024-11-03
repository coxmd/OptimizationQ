using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QueueManagementSystem.MVC.Data.EntityConfigurations;

class TicketEntityTypeConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("QueuedTicket");

        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.PrintTime).HasColumnType("timestamp");

    }
}