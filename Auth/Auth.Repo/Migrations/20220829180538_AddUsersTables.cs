using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Repo.Migrations
{
    public partial class AddUsersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Height = table.Column<double>(type: "double precision", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Members_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Trainers_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Workers_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Trainers",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Workers",
                schema: "Identity");
        }
    }
}
