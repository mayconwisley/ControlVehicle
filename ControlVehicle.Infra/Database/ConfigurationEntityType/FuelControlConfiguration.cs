using ControlVehicle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Infra.Database.ConfigurationEntityType;

public sealed class FuelControlConfiguration : IEntityTypeConfiguration<FuelControl>
{
	public void Configure(EntityTypeBuilder<FuelControl> builder)
	{
		builder.ToTable("fuel_controls");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasColumnType("uuid");

		builder.Property(x => x.VehicleId)
			.HasColumnType("uuid")
			.IsRequired();

		builder.Property(x => x.DriverId)
			.HasColumnType("uuid")
			.IsRequired();

		builder.Property(x => x.InitialKm)
			.HasColumnType("numeric(10,2)")
			.IsRequired();

		builder.Property(x => x.Value)
			.HasColumnType("numeric(10,2)")
			.IsRequired();

		builder.Property(x => x.Date)
			.HasColumnType("timestamp without time zone")
			.IsRequired();

		builder.Property(x => x.Liters)
			.HasColumnType("numeric(10,2)")
			.IsRequired();

		builder.Property(x => x.Description)
			.HasColumnType($"character varying({FuelControl.DescriptionMaxLength})");

		builder.HasIndex(x => x.VehicleId);
		builder.HasIndex(x => x.DriverId);

		builder.HasOne(x => x.Vehicle)
			.WithMany(x => x.FuelControls)
			.HasForeignKey(x => x.VehicleId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Driver)
			.WithMany(x => x.FuelControls)
			.HasForeignKey(x => x.DriverId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
