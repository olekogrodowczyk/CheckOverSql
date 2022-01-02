using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class fixNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_GroupRoles_RoleId",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Invitations",
                newName: "GroupRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_RoleId",
                table: "Invitations",
                newName: "IX_Invitations_GroupRoleId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweredAt",
                table: "Invitations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_GroupRoles_GroupRoleId",
                table: "Invitations",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_GroupRoles_GroupRoleId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "AnsweredAt",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "GroupRoleId",
                table: "Invitations",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_GroupRoleId",
                table: "Invitations",
                newName: "IX_Invitations_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_GroupRoles_RoleId",
                table: "Invitations",
                column: "RoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
