using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlVehicle.Infra.Database.Migrations
{
	/// <inheritdoc />
	public partial class AddDriverCnh : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "driver_cnhs",
				schema: "control_vehicle",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					DriverId = table.Column<Guid>(type: "uuid", nullable: false),
					Cnh = table.Column<string>(type: "character varying(11)", nullable: false),
					DateExpiration = table.Column<DateOnly>(type: "date", nullable: false),
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

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "driver_cnhs",
				schema: "control_vehicle");
		}
	}
}
