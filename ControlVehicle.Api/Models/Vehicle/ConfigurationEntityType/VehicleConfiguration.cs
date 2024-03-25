using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Api.Models.Vehicle.ConfigurationEntityType;

public class VehicleConfiguration : IEntityTypeConfiguration<VehicleModel>
{
    public void Configure(EntityTypeBuilder<VehicleModel> builder)
    {
        builder.Property(p => p.Plate)
            .HasColumnType("VARCHAR(7)")
            .IsRequired();
        builder.Property(p => p.Model)
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        builder.Property(p => p.Renavam)
            .HasColumnType("VARCHAR(11)")
            .IsRequired();
        builder.Property(p => p.Chassi)
            .HasColumnType("VARCHAR(20)");
        builder.Property(p => p.Color)
            .HasColumnType("VARCHAR(30)");
        builder.Property(p => p.Fuel)
            .HasColumnType("VARCHAR(50)");
    }
}
