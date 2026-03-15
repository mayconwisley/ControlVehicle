using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlVehicle.Infra.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "control_vehicle");

        migrationBuilder.CreateTable(
            name: "drivers",
            schema: "control_vehicle",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(150)", nullable: false),
                Cnh = table.Column<string>(type: "character varying(11)", nullable: false),
                CategoryCnh = table.Column<string>(type: "character varying(2)", nullable: false),
                DateExpiration = table.Column<DateOnly>(type: "date", nullable: false),
                Active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_drivers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "vehicles",
            schema: "control_vehicle",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                LicensePlate = table.Column<string>(type: "character varying(7)", nullable: false),
                Model = table.Column<string>(type: "character varying(50)", nullable: false),
                Renavam = table.Column<string>(type: "character varying(11)", nullable: false),
                Chassi = table.Column<string>(type: "character varying(17)", nullable: true),
                Fuel = table.Column<string>(type: "character varying(20)", nullable: false),
                VehicleColor = table.Column<string>(type: "character varying(20)", nullable: false),
                Active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_vehicles", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_drivers_Cnh",
            schema: "control_vehicle",
            table: "drivers",
            column: "Cnh",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_vehicles_LicensePlate",
            schema: "control_vehicle",
            table: "vehicles",
            column: "LicensePlate",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_vehicles_Renavam",
            schema: "control_vehicle",
            table: "vehicles",
            column: "Renavam",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "drivers",
            schema: "control_vehicle");

        migrationBuilder.DropTable(
            name: "vehicles",
            schema: "control_vehicle");
    }
}
