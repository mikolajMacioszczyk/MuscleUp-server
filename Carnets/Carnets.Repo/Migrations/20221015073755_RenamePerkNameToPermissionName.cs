using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class RenamePerkNameToPermissionName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PermissionBase_PermissionName",
                table: "PermissionBase");

            migrationBuilder.RenameColumn(
                name: "PerkName",
                table: "PermissionBase",
                newName: "ClassPermission_PermissionName");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionBase_ClassPermission_PermissionName",
                table: "PermissionBase",
                column: "ClassPermission_PermissionName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PermissionBase_ClassPermission_PermissionName",
                table: "PermissionBase");

            migrationBuilder.RenameColumn(
                name: "ClassPermission_PermissionName",
                table: "PermissionBase",
                newName: "PerkName");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionBase_PermissionName",
                table: "PermissionBase",
                column: "PermissionName",
                unique: true);
        }
    }
}
