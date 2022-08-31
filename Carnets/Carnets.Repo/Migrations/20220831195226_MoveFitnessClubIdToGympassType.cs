using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class MoveFitnessClubIdToGympassType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FitnessClubId",
                table: "Gympasses");

            migrationBuilder.AddColumn<string>(
                name: "FitnessClubId",
                table: "GympassTypes",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FitnessClubId",
                table: "GympassTypes");

            migrationBuilder.AddColumn<string>(
                name: "FitnessClubId",
                table: "Gympasses",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
