using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class AddMembershipTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    FitnessClubId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => new { x.MemberId, x.FitnessClubId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memberships");
        }
    }
}
