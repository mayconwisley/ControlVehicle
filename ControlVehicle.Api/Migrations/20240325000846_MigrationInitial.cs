using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlVehicle.Api.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    CNH = table.Column<string>(type: "VARCHAR(12)", nullable: false),
                    CategoryCNH = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plate = table.Column<string>(type: "VARCHAR(7)", nullable: false),
                    Model = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Chassi = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    Renavam = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    Fuel = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    Color = table.Column<string>(type: "VARCHAR(30)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
