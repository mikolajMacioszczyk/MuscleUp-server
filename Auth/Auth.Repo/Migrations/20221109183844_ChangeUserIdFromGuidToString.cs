using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Repo.Migrations
{
    public partial class ChangeUserIdFromGuidToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Identity",
                table: "AuthTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Identity",
                table: "AuthTokens",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
