using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fixGroupRoleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_GroupRoles_RoleId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Assignments",
                newName: "GroupRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_RoleId",
                table: "Assignments",
                newName: "IX_Assignments_GroupRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_GroupRoles_GroupRoleId",
                table: "Assignments",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_GroupRoles_GroupRoleId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "GroupRoleId",
                table: "Assignments",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_GroupRoleId",
                table: "Assignments",
                newName: "IX_Assignments_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_GroupRoles_RoleId",
                table: "Assignments",
                column: "RoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
