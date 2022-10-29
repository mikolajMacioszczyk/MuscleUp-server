using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class DeletedFitnessClub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FitnessClubs_FitnessClubName",
                table: "FitnessClubs");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FitnessClubs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FitnessClubs");

            migrationBuilder.CreateIndex(
                name: "IX_FitnessClubs_FitnessClubName",
                table: "FitnessClubs",
                column: "FitnessClubName",
                unique: true);
        }
    }
}
