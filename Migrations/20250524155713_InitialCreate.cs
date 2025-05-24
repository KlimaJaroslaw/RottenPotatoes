using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RottenPotatoes.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    MovieID = table.Column<int>(name: "Movie_ID", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ProductionDate = table.Column<DateTime>(name: "Production_Date", type: "TEXT", nullable: false),
                    Director = table.Column<string>(type: "TEXT", nullable: false),
                    Producer = table.Column<string>(type: "TEXT", nullable: false),
                    ScreenWriter = table.Column<string>(type: "TEXT", nullable: false),
                    Synopsis = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.MovieID);
                });

            migrationBuilder.CreateTable(
                name: "Watchlist",
                columns: table => new
                {
                    WatchlistID = table.Column<int>(name: "Watchlist_ID", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(name: "User_ID", type: "INTEGER", nullable: false),
                    MovieID = table.Column<int>(name: "Movie_ID", type: "INTEGER", nullable: false),
                    AddedDate = table.Column<DateTime>(name: "Added_Date", type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieID1 = table.Column<int>(name: "Movie_ID1", type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlist", x => x.WatchlistID);
                    table.ForeignKey(
                        name: "FK_Watchlist_Movie_Movie_ID1",
                        column: x => x.MovieID1,
                        principalTable: "Movie",
                        principalColumn: "Movie_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Watchlist_Movie_ID1",
                table: "Watchlist",
                column: "Movie_ID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Watchlist");

            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
