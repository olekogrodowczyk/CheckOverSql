using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class convertDatabaseInto1NFWithValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                table: "Databases");

            migrationBuilder.DropColumn(
                name: "ConnectionStringAdmin",
                table: "Databases");

            migrationBuilder.AddColumn<string>(
                name: "Database",
                table: "Databases",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Databases",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Server",
                table: "Databases",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Databases",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Database",
                table: "Databases");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Databases");

            migrationBuilder.DropColumn(
                name: "Server",
                table: "Databases");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Databases");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                table: "Databases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionStringAdmin",
                table: "Databases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
