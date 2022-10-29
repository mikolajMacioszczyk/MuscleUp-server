using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class Interval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingValidityPeriodInSeconds",
                table: "Gympasses");

            migrationBuilder.RenameColumn(
                name: "ValidityPeriodInSeconds",
                table: "GympassTypes",
                newName: "IntervalCount");

            migrationBuilder.AddColumn<int>(
                name: "Interval",
                table: "GympassTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interval",
                table: "GympassTypes");

            migrationBuilder.RenameColumn(
                name: "IntervalCount",
                table: "GympassTypes",
                newName: "ValidityPeriodInSeconds");

            migrationBuilder.AddColumn<int>(
                name: "RemainingValidityPeriodInSeconds",
                table: "Gympasses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
