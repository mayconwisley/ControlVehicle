using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Infra.Database.ConfigurationEntityType;

public sealed class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
	public void Configure(EntityTypeBuilder<Driver> builder)
	{
		builder.ToTable("drivers");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasColumnType("uuid");

		builder.Property(x => x.Name)
			.HasColumnType("character varying(150)")
			.IsRequired();

		builder.Property(x => x.Cnh)
			.HasConversion(
				v => v.Number,
				v => Cnh.Create(v))
			.HasColumnType("character varying(11)")
			.IsRequired();

		builder.Property(x => x.CategoryCnh)
			.HasConversion(
				v => v.Value,
				v => CategoryCnh.Create(v))
			.HasColumnType("character varying(2)")
			.IsRequired();

		builder.Property(x => x.DateExpiration)
			.HasColumnType("date")
			.IsRequired();

		builder.Property(x => x.Active)
			.HasColumnType("boolean")
			.IsRequired();

		// opcional: evita CNH duplicada
		builder.HasIndex(x => x.Cnh).IsUnique();
	}
}