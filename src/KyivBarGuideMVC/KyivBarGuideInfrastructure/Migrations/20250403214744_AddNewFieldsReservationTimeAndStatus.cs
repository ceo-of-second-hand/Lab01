using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsReservationTimeAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reservations",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "Reservations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(20, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Reservations");
        }
    }
}
