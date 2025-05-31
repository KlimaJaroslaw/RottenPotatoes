using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace RottenPotatoes.Migrations
{
    /// <inheritdoc />
    public partial class MovieInserts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Login_Hash",
                table: "Users",
                column: "Login_Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Description",
                table: "Permissions",
                column: "Description",
                unique: true);

            ExecuteSqlFile(migrationBuilder, "insert_movies.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login_Hash",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Description",
                table: "Permissions");

            ExecuteSqlFile(migrationBuilder, "delete_movies.sql");
        }

        private void ExecuteSqlFile(MigrationBuilder builder, string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"RottenPotatoes.Migrations.{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(
                        $"Embedded resource '{resourceName}' not found. " +
                        $"Valid resources: {string.Join(", ", assembly.GetManifestResourceNames())}"
                    );
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    builder.Sql(sql);
                }
            }
        }
    }
}
