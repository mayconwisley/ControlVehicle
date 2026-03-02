using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlVehicle.Infra.Database.ConfigurationEntityType;

public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
	public void Configure(EntityTypeBuilder<Vehicle> builder)
	{
		builder.ToTable("vehicles");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasColumnType("uuid");

		builder.Property(x => x.LicensePlate)
			.HasConversion(
				v => v.Value,
				v => LicensePlate.Create(v))
			.HasColumnType("character varying(7)")
			.IsRequired();

		builder.Property(x => x.Model)
			.HasColumnType("character varying(50)")
			.IsRequired();

		builder.Property(x => x.Renavam)
			.HasConversion(
				v => v.Value,
				v => Renavam.Create(v))
			.HasColumnType("character varying(11)")
			.IsRequired();

		builder.Property(x => x.Chassi)
			.HasConversion(
				v => v == null ? null : v.Value,
				v => v == null ? null : Chassi.Create(v))
			.HasColumnType("character varying(17)");

		// enums como string no Postgres (bem legível)
		builder.Property(x => x.Fuel)
			.HasConversion<string>()
			.HasColumnType("character varying(20)")
			.IsRequired();

		builder.Property(x => x.VehicleColor)
			.HasConversion<string>()
			.HasColumnType("character varying(20)")
			.IsRequired();

		builder.Property(x => x.Active)
			.HasColumnType("boolean")
			.IsRequired();

		// Índices/Unique constraints úteis
		builder.HasIndex(x => x.LicensePlate).IsUnique();
		builder.HasIndex(x => x.Renavam).IsUnique();

		// Se Chassi for único quando informado (parcial unique index)
		// (EF ainda não faz nativo perfeito em todas versões; via migration manual fica melhor)
	}
}