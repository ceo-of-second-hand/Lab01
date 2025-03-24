using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsBarAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Bars",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Bars");
        }
    }
}
