using ControlVehicle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Infra.Database.ConfigurationEntityType;

public sealed class MaintenanceControlConfiguration : IEntityTypeConfiguration<MaintenanceControl>
{
    public void Configure(EntityTypeBuilder<MaintenanceControl> builder)
    {
        builder.ToTable("maintenance_controls");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnType("uuid");

        builder.Property(x => x.VehicleId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.Date)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType($"character varying({MaintenanceControl.DescriptionMaxLength})");

        builder.HasIndex(x => x.VehicleId);

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.MaintenanceControls)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
