using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Repo.Migrations
{
    public partial class AddAuthTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthTokens",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessTokenId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshTokenId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthTokens",
                schema: "Identity");
        }
    }
}
