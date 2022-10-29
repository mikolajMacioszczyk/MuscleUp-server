using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Repo.Migrations
{
    public partial class MeasuresSuffixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                schema: "Identity",
                table: "Members",
                newName: "WeightInKg");

            migrationBuilder.RenameColumn(
                name: "Height",
                schema: "Identity",
                table: "Members",
                newName: "HeightInCm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeightInKg",
                schema: "Identity",
                table: "Members",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "HeightInCm",
                schema: "Identity",
                table: "Members",
                newName: "Height");
        }
    }
}
