using ControlVehicle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Infra.Database.ConfigurationEntityType;

public sealed class VehicleControlConfiguration : IEntityTypeConfiguration<VehicleControl>
{
	public void Configure(EntityTypeBuilder<VehicleControl> builder)
	{
		builder.ToTable("vehicle_controls");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasColumnType("uuid");

		builder.Property(x => x.VehicleId)
			.HasColumnType("uuid")
			.IsRequired();

		builder.Property(x => x.DriverId)
			.HasColumnType("uuid")
			.IsRequired();

		builder.Property(x => x.DepartureDate)
			.HasColumnType("timestamp without time zone")
			.IsRequired();

		builder.Property(x => x.ArrivalDate)
			.HasColumnType("timestamp without time zone")
			.IsRequired();

		builder.Property(x => x.InitialKm)
			.HasColumnType("numeric(10,2)")
			.IsRequired();

		builder.Property(x => x.FinalKm)
			.HasColumnType("numeric(10,2)")
			.IsRequired();

		builder.Property(x => x.Description)
			.HasColumnType($"character varying({VehicleControl.DescriptionMaxLength})")
			.IsRequired();

		builder.HasIndex(x => x.VehicleId);
		builder.HasIndex(x => x.DriverId);

		builder.HasOne(x => x.Vehicle)
			.WithMany(x => x.Controls)
			.HasForeignKey(x => x.VehicleId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Driver)
			.WithMany(x => x.Controls)
			.HasForeignKey(x => x.DriverId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
