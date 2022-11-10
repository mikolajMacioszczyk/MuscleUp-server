using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class AddUserInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInvitations",
                columns: table => new
                {
                    InvitationId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    UserType = table.Column<byte>(type: "smallint", nullable: false),
                    FitnessClubId = table.Column<string>(type: "character varying(36)", nullable: false),
                    ExpirationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvitations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_UserInvitations_FitnessClubs_FitnessClubId",
                        column: x => x.FitnessClubId,
                        principalTable: "FitnessClubs",
                        principalColumn: "FitnessClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitations_FitnessClubId",
                table: "UserInvitations",
                column: "FitnessClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInvitations");
        }
    }
}
