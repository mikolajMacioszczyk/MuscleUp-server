using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class RenameFromToJoiningDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "From",
                table: "Memberships",
                newName: "JoiningDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoiningDate",
                table: "Memberships",
                newName: "From");
        }
    }
}
