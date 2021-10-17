using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class comparisonModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comparisons_Solutions_Solution1Id",
                table: "Comparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_Comparisons_Solutions_Solution2Id",
                table: "Comparisons");

            migrationBuilder.RenameColumn(
                name: "Solution2Id",
                table: "Comparisons",
                newName: "SolutionId");

            migrationBuilder.RenameColumn(
                name: "Solution1Id",
                table: "Comparisons",
                newName: "ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_Comparisons_Solution2Id",
                table: "Comparisons",
                newName: "IX_Comparisons_SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Comparisons_Solution1Id",
                table: "Comparisons",
                newName: "IX_Comparisons_ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comparisons_Exercises_ExerciseId",
                table: "Comparisons",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comparisons_Solutions_SolutionId",
                table: "Comparisons",
                column: "SolutionId",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comparisons_Exercises_ExerciseId",
                table: "Comparisons");

            migrationBuilder.DropForeignKey(
                name: "FK_Comparisons_Solutions_SolutionId",
                table: "Comparisons");

            migrationBuilder.RenameColumn(
                name: "SolutionId",
                table: "Comparisons",
                newName: "Solution2Id");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "Comparisons",
                newName: "Solution1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comparisons_SolutionId",
                table: "Comparisons",
                newName: "IX_Comparisons_Solution2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comparisons_ExerciseId",
                table: "Comparisons",
                newName: "IX_Comparisons_Solution1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comparisons_Solutions_Solution1Id",
                table: "Comparisons",
                column: "Solution1Id",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comparisons_Solutions_Solution2Id",
                table: "Comparisons",
                column: "Solution2Id",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
