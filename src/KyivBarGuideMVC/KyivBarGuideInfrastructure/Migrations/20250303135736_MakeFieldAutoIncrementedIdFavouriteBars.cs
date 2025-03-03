using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeFieldAutoIncrementedIdFavouriteBars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // remove the old primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Favourite_bars",
                table: "FavouriteBars");

            // remove the old column
            migrationBuilder.DropColumn(
                name: "id",
                table: "FavouriteBars");

            // create the new column with auto-increment
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "FavouriteBars",
                type: "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            // add the primary key constraint back on the new column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Favourite_bars",
                table: "FavouriteBars",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Favourite_bars",
                table: "FavouriteBars");

            // remove the new column
            migrationBuilder.DropColumn(
                name: "id",
                table: "FavouriteBars");

            // restore the old structure (without auto-increment), if needed
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "FavouriteBars",
                type: "int",
                nullable: false);

            // add the primary key constraint back on the old column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Favourite_bars",
                table: "FavouriteBars",
                column: "id");
        }
    }
}

