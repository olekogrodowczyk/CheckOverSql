using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addDatabaseQueryRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Queries_DatabaseId",
                table: "Queries",
                column: "DatabaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Databases_DatabaseId",
                table: "Queries",
                column: "DatabaseId",
                principalTable: "Databases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Databases_DatabaseId",
                table: "Queries");

            migrationBuilder.DropIndex(
                name: "IX_Queries_DatabaseId",
                table: "Queries");
        }
    }
}
