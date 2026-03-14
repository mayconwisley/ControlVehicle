using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlVehicle.Infra.Database.Migrations;

/// <inheritdoc />
public partial class AddVehicleControl : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "driver_cnhs",
			schema: "control_vehicle");

		migrationBuilder.CreateTable(
			name: "vehicle_controls",
			schema: "control_vehicle",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
				DriverId = table.Column<Guid>(type: "uuid", nullable: false),
				DepartureDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
				ArrivalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
				InitialKm = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
				FinalKm = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
				Description = table.Column<string>(type: "character varying(1000)", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_vehicle_controls", x => x.Id);
				table.ForeignKey(
					name: "FK_vehicle_controls_drivers_DriverId",
					column: x => x.DriverId,
					principalSchema: "control_vehicle",
					principalTable: "drivers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Restrict);
				table.ForeignKey(
					name: "FK_vehicle_controls_vehicles_VehicleId",
					column: x => x.VehicleId,
					principalSchema: "control_vehicle",
					principalTable: "vehicles",
					principalColumn: "Id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.CreateIndex(
			name: "IX_vehicle_controls_DriverId",
			schema: "control_vehicle",
			table: "vehicle_controls",
			column: "DriverId");

		migrationBuilder.CreateIndex(
			name: "IX_vehicle_controls_VehicleId",
			schema: "control_vehicle",
			table: "vehicle_controls",
			column: "VehicleId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "vehicle_controls",
			schema: "control_vehicle");

		migrationBuilder.CreateTable(
			name: "driver_cnhs",
			schema: "control_vehicle",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				Cnh = table.Column<string>(type: "character varying(11)", nullable: false),
				DateExpiration = table.Column<DateOnly>(type: "date", nullable: false),
				DriverId = table.Column<Guid>(type: "uuid", nullable: false),
				Status = table.Column<string>(type: "character varying(20)", nullable: false, defaultValue: "Active")
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_driver_cnhs", x => x.Id);
				table.ForeignKey(
					name: "FK_driver_cnhs_drivers_DriverId",
					column: x => x.DriverId,
					principalSchema: "control_vehicle",
					principalTable: "drivers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.CreateIndex(
			name: "IX_driver_cnhs_Cnh",
			schema: "control_vehicle",
			table: "driver_cnhs",
			column: "Cnh",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_driver_cnhs_DriverId",
			schema: "control_vehicle",
			table: "driver_cnhs",
			column: "DriverId",
			unique: true);
	}
}
