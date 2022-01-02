using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangeDeleteBehaviorToInvitationGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Groups_GroupId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Solvings_Assignments_AssignmentId",
                table: "Solvings");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Groups_GroupId",
                table: "Invitations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Solvings_Assignments_AssignmentId",
                table: "Solvings",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Groups_GroupId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Solvings_Assignments_AssignmentId",
                table: "Solvings");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Groups_GroupId",
                table: "Invitations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solvings_Assignments_AssignmentId",
                table: "Solvings",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
