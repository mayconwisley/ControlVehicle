using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlVehicle.Infra.Database.Migrations;

/// <inheritdoc />
public partial class AddFuelControl : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "fuel_controls",
            schema: "control_vehicle",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                InitialKm = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                Value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Liters = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                Description = table.Column<string>(type: "character varying(500)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_fuel_controls", x => x.Id);
                table.ForeignKey(
                    name: "FK_fuel_controls_drivers_DriverId",
                    column: x => x.DriverId,
                    principalSchema: "control_vehicle",
                    principalTable: "drivers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_fuel_controls_vehicles_VehicleId",
                    column: x => x.VehicleId,
                    principalSchema: "control_vehicle",
                    principalTable: "vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_fuel_controls_DriverId",
            schema: "control_vehicle",
            table: "fuel_controls",
            column: "DriverId");

        migrationBuilder.CreateIndex(
            name: "IX_fuel_controls_VehicleId",
            schema: "control_vehicle",
            table: "fuel_controls",
            column: "VehicleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "fuel_controls",
            schema: "control_vehicle");
    }
}
