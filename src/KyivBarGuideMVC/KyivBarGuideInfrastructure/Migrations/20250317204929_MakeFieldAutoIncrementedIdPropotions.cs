using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeFieldAutoIncrementedIdProportions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Видаляємо старий первинний ключ
            migrationBuilder.DropPrimaryKey(
                name: "PK_Proportions",
                table: "Proportions");

            // Видаляємо старе поле Id
            migrationBuilder.DropColumn(
                name: "id",
                table: "Proportions");

            // Додаємо нове поле Id з автоінкрементом
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Proportions",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Додаємо первинний ключ на нове поле Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_Proportions",
                table: "Proportions",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Видаляємо первинний ключ
            migrationBuilder.DropPrimaryKey(
                name: "PK_Proportions",
                table: "Proportions");

            // Видаляємо нове поле Id з автоінкрементом
            migrationBuilder.DropColumn(
                name: "id",
                table: "Proportions");

            // Відновлюємо старе поле Id (без автоінкременту)
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Proportions",
                type: "int",
                nullable: false);

            // Додаємо первинний ключ на старе поле Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_Proportions",
                table: "Proportions",
                column: "id");
        }
    }
}