using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fixRelationBetweenSolutionAndComparison : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comparisons_SolutionId",
                table: "Comparisons");

            migrationBuilder.CreateIndex(
                name: "IX_Comparisons_SolutionId",
                table: "Comparisons",
                column: "SolutionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comparisons_SolutionId",
                table: "Comparisons");

            migrationBuilder.CreateIndex(
                name: "IX_Comparisons_SolutionId",
                table: "Comparisons",
                column: "SolutionId");
        }
    }
}
