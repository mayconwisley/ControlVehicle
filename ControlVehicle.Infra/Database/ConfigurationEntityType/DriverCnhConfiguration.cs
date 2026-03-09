using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Enums;
using ControlVehicle.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Infra.Database.ConfigurationEntityType;

public sealed class DriverCnhConfiguration : IEntityTypeConfiguration<DriverCnh>
{
	public void Configure(EntityTypeBuilder<DriverCnh> builder)
	{
		builder.ToTable("driver_cnhs");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasColumnType("uuid");

		builder.Property(x => x.DriverId)
			.HasColumnType("uuid")
			.IsRequired();

		builder.Property(x => x.Cnh)
			.HasConversion(
				v => v.Number,
				v => Cnh.Create(v))
			.HasColumnType("character varying(11)")
			.IsRequired();

		builder.Property(x => x.DateExpiration)
			.HasColumnType("date")
			.IsRequired();

		builder.Property(x => x.Status)
			.HasConversion<string>()
			.HasColumnType("character varying(20)")
			.HasDefaultValue(EmploymentStatusEnum.Active)
			.IsRequired();

		builder.HasOne<Driver>()
			.WithMany()
			.HasForeignKey(x => x.DriverId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(x => x.DriverId).IsUnique();
		builder.HasIndex(x => x.Cnh).IsUnique();
	}
}
