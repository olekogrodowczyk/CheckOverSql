using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class SetDeleteingLogicWithDatabases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Databases_DatabaseId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Databases_DatabaseId",
                table: "Queries");

            migrationBuilder.AlterColumn<int>(
                name: "DatabaseId",
                table: "Exercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Databases_DatabaseId",
                table: "Exercises",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Databases_DatabaseId",
                table: "Queries",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Databases_DatabaseId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Databases_DatabaseId",
                table: "Queries");

            migrationBuilder.AlterColumn<int>(
                name: "DatabaseId",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Databases_DatabaseId",
                table: "Exercises",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Databases_DatabaseId",
                table: "Queries",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
