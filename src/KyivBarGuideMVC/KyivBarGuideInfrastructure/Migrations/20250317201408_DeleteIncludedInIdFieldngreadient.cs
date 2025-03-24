using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIncludedInIdFieldngreadient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "included_in_id",
                table: "Ingredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "included_in_id",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
