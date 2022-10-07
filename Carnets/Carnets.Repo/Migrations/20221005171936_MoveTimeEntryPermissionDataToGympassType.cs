using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class MoveTimeEntryPermissionDataToGympassType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableEntryFrom",
                table: "PermissionBase");

            migrationBuilder.DropColumn(
                name: "EnableEntryTo",
                table: "PermissionBase");

            migrationBuilder.AddColumn<int>(
                name: "EnableEntryFromInMinutes",
                table: "GympassTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnableEntryToInMinutes",
                table: "GympassTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableEntryFromInMinutes",
                table: "GympassTypes");

            migrationBuilder.DropColumn(
                name: "EnableEntryToInMinutes",
                table: "GympassTypes");

            migrationBuilder.AddColumn<byte>(
                name: "EnableEntryFrom",
                table: "PermissionBase",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "EnableEntryTo",
                table: "PermissionBase",
                type: "smallint",
                nullable: true);
        }
    }
}
