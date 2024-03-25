using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Api.Models.Driver.ConfigurationEntityType;

public class DriverConfiguration : IEntityTypeConfiguration<DriverModel>
{
    public void Configure(EntityTypeBuilder<DriverModel> builder)
    {
        builder.Property(p => p.Name)
            .HasColumnType("VARCHAR(150)")
            .IsRequired();
        builder.Property(p => p.CNH)
            .HasColumnType("VARCHAR(12)")
            .IsRequired();
        builder.Property(p => p.CategoryCNH)
            .HasColumnType("VARCHAR(2)")
            .IsRequired();
        builder.Property(p => p.DateExpiration)
            .HasColumnType("DATETIME")
            .IsRequired();
    }
}
