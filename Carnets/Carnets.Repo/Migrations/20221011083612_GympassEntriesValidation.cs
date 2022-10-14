using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class GympassEntriesValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedEntries",
                table: "PermissionBase");

            migrationBuilder.DropColumn(
                name: "AllowedEntriesCooldown",
                table: "PermissionBase");

            migrationBuilder.DropColumn(
                name: "CooldownType",
                table: "PermissionBase");

            migrationBuilder.AddColumn<int>(
                name: "AllowedEntries",
                table: "GympassTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValidationType",
                table: "GympassTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingEntries",
                table: "Gympasses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedEntries",
                table: "GympassTypes");

            migrationBuilder.DropColumn(
                name: "ValidationType",
                table: "GympassTypes");

            migrationBuilder.DropColumn(
                name: "RemainingEntries",
                table: "Gympasses");

            migrationBuilder.AddColumn<int>(
                name: "AllowedEntries",
                table: "PermissionBase",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AllowedEntriesCooldown",
                table: "PermissionBase",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "CooldownType",
                table: "PermissionBase",
                type: "smallint",
                nullable: true);
        }
    }
}
