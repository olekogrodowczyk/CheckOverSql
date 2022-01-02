using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fixGroupRoleNameinGroupRolePermissionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_GroupRoles_RoleId",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RolePermissions",
                newName: "GroupRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_GroupRoleId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnsweredAt",
                table: "Invitations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_GroupRoles_GroupRoleId",
                table: "RolePermissions",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_GroupRoles_GroupRoleId",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "GroupRoleId",
                table: "RolePermissions",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_GroupRoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnsweredAt",
                table: "Invitations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_GroupRoles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
