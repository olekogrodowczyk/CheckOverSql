using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class FixGroupRolePermissionEntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_GroupRoles_GroupRoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "GroupRolePermissions");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "GroupRolePermissions",
                newName: "IX_GroupRolePermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_GroupRoleId",
                table: "GroupRolePermissions",
                newName: "IX_GroupRolePermissions_GroupRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupRolePermissions",
                table: "GroupRolePermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRolePermissions_GroupRoles_GroupRoleId",
                table: "GroupRolePermissions",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRolePermissions_Permissions_PermissionId",
                table: "GroupRolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRolePermissions_GroupRoles_GroupRoleId",
                table: "GroupRolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRolePermissions_Permissions_PermissionId",
                table: "GroupRolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupRolePermissions",
                table: "GroupRolePermissions");

            migrationBuilder.RenameTable(
                name: "GroupRolePermissions",
                newName: "RolePermissions");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRolePermissions_PermissionId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRolePermissions_GroupRoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_GroupRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_GroupRoles_GroupRoleId",
                table: "RolePermissions",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
