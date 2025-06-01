using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RottenPotatoes.Migrations
{
    /// <inheritdoc />
    public partial class TableInserts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email_Hash",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reviews",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlist_User_ID",
                table: "Watchlist",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlist_Users_User_ID",
                table: "Watchlist",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);


            migrationBuilder.Sql("INSERT INTO Permissions (Description) VALUES ('System Admin');");
            migrationBuilder.Sql("INSERT INTO Permissions (Description) VALUES ('Reviewer');");
            migrationBuilder.Sql("INSERT INTO Permissions (Description) VALUES ('Casual');");
            migrationBuilder.Sql("INSERT INTO Users (Login_Hash, Password_Hash, Permission_ID) VALUES ('admin0', 'admin0', (SELECT Permission_ID FROM Permissions WHERE Description = 'System Admin'))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlist_Users_User_ID",
                table: "Watchlist");

            migrationBuilder.DropIndex(
                name: "IX_Watchlist_User_ID",
                table: "Watchlist");

            migrationBuilder.DropColumn(
                name: "Email_Hash",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reviews",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.Sql("DELETE FROM Permissions WHERE Description = 'System Admin';");
            migrationBuilder.Sql("DELETE FROM Permissions WHERE Description = 'Reviewer';");
            migrationBuilder.Sql("DELETE FROM Permissions WHERE Description = 'Casual';");
            migrationBuilder.Sql("DELETE FROM Users WHERE Login_Hash = 'admin0';");
        }
    }
}
