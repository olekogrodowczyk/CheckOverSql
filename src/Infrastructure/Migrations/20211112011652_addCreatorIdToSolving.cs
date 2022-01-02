using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addCreatorIdToSolving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Solvings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Solvings_CreatorId",
                table: "Solvings",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solvings_Users_CreatorId",
                table: "Solvings",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solvings_Users_CreatorId",
                table: "Solvings");

            migrationBuilder.DropIndex(
                name: "IX_Solvings_CreatorId",
                table: "Solvings");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Solvings");
        }
    }
}
