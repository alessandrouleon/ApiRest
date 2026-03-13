using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIRest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameAndIsActiveToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // Assign unique usernames to existing rows using their Id
            migrationBuilder.Sql(
                "UPDATE [Users] SET [Username] = 'user_' + LOWER(REPLACE(CAST([Id] AS nvarchar(36)), '-', '')) WHERE [Username] = ''");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");
        }
    }
}
