using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RottenPotatoes.Migrations
{
    /// <inheritdoc />
    public partial class AddedApiTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiToken",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiToken",
                table: "Users");
        }
    }
}
