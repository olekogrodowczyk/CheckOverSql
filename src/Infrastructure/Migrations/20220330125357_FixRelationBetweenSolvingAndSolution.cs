using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class FixRelationBetweenSolvingAndSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Solvings_SolvingId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_SolvingId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "SolvingId",
                table: "Solutions");

            migrationBuilder.AddColumn<int>(
                name: "SolutionId",
                table: "Solvings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solvings_SolutionId",
                table: "Solvings",
                column: "SolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solvings_Solutions_SolutionId",
                table: "Solvings",
                column: "SolutionId",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solvings_Solutions_SolutionId",
                table: "Solvings");

            migrationBuilder.DropIndex(
                name: "IX_Solvings_SolutionId",
                table: "Solvings");

            migrationBuilder.DropColumn(
                name: "SolutionId",
                table: "Solvings");

            migrationBuilder.AddColumn<int>(
                name: "SolvingId",
                table: "Solutions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_SolvingId",
                table: "Solutions",
                column: "SolvingId",
                unique: true,
                filter: "[SolvingId] IS NOT NULL");

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
