using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlVehicle.Infra.Database.Migrations;

/// <inheritdoc />
public partial class AddMaintenanceControl : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "maintenance_controls",
            schema: "control_vehicle",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                Description = table.Column<string>(type: "character varying(500)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_maintenance_controls", x => x.Id);
                table.ForeignKey(
                    name: "FK_maintenance_controls_vehicles_VehicleId",
                    column: x => x.VehicleId,
                    principalSchema: "control_vehicle",
                    principalTable: "vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_maintenance_controls_VehicleId",
            schema: "control_vehicle",
            table: "maintenance_controls",
            column: "VehicleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "maintenance_controls",
            schema: "control_vehicle");
    }
}
