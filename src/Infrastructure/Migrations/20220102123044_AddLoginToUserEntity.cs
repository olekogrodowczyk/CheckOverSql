using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddLoginToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions",
                column: "SolvingId",
                principalTable: "Solvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions",
                column: "SolvingId",
                principalTable: "Solvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
