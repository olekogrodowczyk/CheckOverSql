using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangeDeleteBehaviorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions",
                column: "SolvingId",
                principalTable: "Solvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions",
                column: "SolvingId",
                principalTable: "Solvings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
