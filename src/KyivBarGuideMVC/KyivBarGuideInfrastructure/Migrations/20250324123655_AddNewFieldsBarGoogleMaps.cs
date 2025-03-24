using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsBarGoogleMaps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
            */
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Bars",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Bars",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Bars");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Bars");

            /*
            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
            */
        }
    }
}
