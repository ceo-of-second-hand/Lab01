using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyivBarGuideInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.CreateTable(
                name: "Bars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    theme = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    rating = table.Column<decimal>(type: "numeric(3,2)", nullable: true),
                    picture = table.Column<string>(type: "varchar(2083)", unicode: false, maxLength: 2083, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Musicians",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musician", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    work_in_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    experience = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.id);
                    table.ForeignKey(
                        name: "Run",
                        column: x => x.work_in_id,
                        principalTable: "Bars",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Cocktails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    sells_in = table.Column<int>(type: "int", nullable: false),
                    picture = table.Column<string>(type: "varchar(2083)", unicode: false, maxLength: 2083, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cocktail", x => x.id);
                    table.ForeignKey(
                        name: "Sell",
                        column: x => x.sells_in,
                        principalTable: "Bars",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FavouriteBars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    added_by_id = table.Column<int>(type: "int", nullable: false),
                    added_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourite_bars", x => x.id);
                    table.ForeignKey(
                        name: "Add",
                        column: x => x.added_by_id,
                        principalTable: "Clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Change",
                        column: x => x.added_id,
                        principalTable: "Bars",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    written_by_id = table.Column<int>(type: "int", nullable: false),
                    rates_id = table.Column<int>(type: "int", nullable: false),
                    five_star_rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.id);
                    table.ForeignKey(
                        name: "Leave",
                        column: x => x.written_by_id,
                        principalTable: "Clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Rate",
                        column: x => x.rates_id,
                        principalTable: "Bars",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    written_by_id = table.Column<int>(type: "int", nullable: false),
                    reviewed_by_id = table.Column<int>(type: "int", nullable: false),
                    genre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    streaming_service_link = table.Column<string>(type: "varchar(2083)", unicode: false, maxLength: 2083, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.id);
                    table.ForeignKey(
                        name: "Choose",
                        column: x => x.written_by_id,
                        principalTable: "Musicians",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Evaluate",
                        column: x => x.reviewed_by_id,
                        principalTable: "Admins",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    included_in_id = table.Column<int>(type: "int", nullable: false),
                    added_by_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.id);
                    table.ForeignKey(
                        name: "Mix",
                        column: x => x.added_by_id,
                        principalTable: "Admins",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reserved_by_id = table.Column<int>(type: "int", nullable: true),
                    reserved_in_id = table.Column<int>(type: "int", nullable: false),
                    confirmed_by_id = table.Column<int>(type: "int", nullable: true),
                    smoker_status = table.Column<bool>(type: "bit", nullable: false),
                    concert_visit = table.Column<bool>(type: "bit", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.id);
                    table.ForeignKey(
                        name: "Confirm",
                        column: x => x.confirmed_by_id,
                        principalTable: "Admins",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Make",
                        column: x => x.reserved_by_id,
                        principalTable: "Clients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Proportions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    amount_in_id = table.Column<int>(type: "int", nullable: false),
                    set_by_id = table.Column<int>(type: "int", nullable: false),
                    amount_of_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proportions", x => x.id);
                    table.ForeignKey(
                        name: "Comprise",
                        column: x => x.amount_of_id,
                        principalTable: "Ingredients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Contain",
                        column: x => x.amount_in_id,
                        principalTable: "Cocktails",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Set",
                        column: x => x.set_by_id,
                        principalTable: "Admins",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_work_in_id",
                table: "Admins",
                column: "work_in_id");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_reviewed_by_id",
                table: "Albums",
                column: "reviewed_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_written_by_id",
                table: "Albums",
                column: "written_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cocktails_sells_in",
                table: "Cocktails",
                column: "sells_in");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteBars_added_by_id",
                table: "FavouriteBars",
                column: "added_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteBars_added_id",
                table: "FavouriteBars",
                column: "added_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_added_by_id",
                table: "Ingredients",
                column: "added_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Proportions_amount_in_id",
                table: "Proportions",
                column: "amount_in_id");

            migrationBuilder.CreateIndex(
                name: "IX_Proportions_amount_of_id",
                table: "Proportions",
                column: "amount_of_id");

            migrationBuilder.CreateIndex(
                name: "IX_Proportions_set_by_id",
                table: "Proportions",
                column: "set_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_confirmed_by_id",
                table: "Reservations",
                column: "confirmed_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_reserved_by_id",
                table: "Reservations",
                column: "reserved_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_rates_id",
                table: "Reviews",
                column: "rates_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_written_by_id",
                table: "Reviews",
                column: "written_by_id");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "FavouriteBars");

            migrationBuilder.DropTable(
                name: "Proportions");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Musicians");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Cocktails");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Bars");
            */
        }
    }
}
