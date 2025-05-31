using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RottenPotatoes.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionID = table.Column<int>(name: "Permission_ID", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(name: "User_ID", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoginHash = table.Column<string>(name: "Login_Hash", type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(name: "Password_Hash", type: "TEXT", nullable: false),
                    PermissionID = table.Column<int>(name: "Permission_ID", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Permissions_Permission_ID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "Permission_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(name: "Review_ID", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(name: "User_ID", type: "INTEGER", nullable: false),
                    MovieID = table.Column<int>(name: "Movie_ID", type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Reviews_Movie_Movie_ID",
                        column: x => x.MovieID,
                        principalTable: "Movie",
                        principalColumn: "Movie_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_User_ID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Movie_ID",
                table: "Reviews",
                column: "Movie_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_User_ID",
                table: "Reviews",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Permission_ID",
                table: "Users",
                column: "Permission_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
