using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class AddFitnessClubIdToPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FitnessClubId",
                table: "PermissionBase",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FitnessClubId",
                table: "PermissionBase");
        }
    }
}
