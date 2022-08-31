using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FitnessClubs",
                columns: table => new
                {
                    FitnessClubId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FitnessClubName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessClubs", x => x.FitnessClubId);
                });

            migrationBuilder.CreateTable(
                name: "TrainerEmployments",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FitnessClubId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerEmployments", x => new { x.UserId, x.FitnessClubId });
                    table.ForeignKey(
                        name: "FK_TrainerEmployments_FitnessClubs_FitnessClubId",
                        column: x => x.FitnessClubId,
                        principalTable: "FitnessClubs",
                        principalColumn: "FitnessClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerEmployments",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FitnessClubId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerEmployments", x => new { x.UserId, x.FitnessClubId });
                    table.ForeignKey(
                        name: "FK_WorkerEmployments_FitnessClubs_FitnessClubId",
                        column: x => x.FitnessClubId,
                        principalTable: "FitnessClubs",
                        principalColumn: "FitnessClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitnessClubs_FitnessClubName",
                table: "FitnessClubs",
                column: "FitnessClubName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainerEmployments_FitnessClubId",
                table: "TrainerEmployments",
                column: "FitnessClubId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerEmployments_FitnessClubId",
                table: "WorkerEmployments",
                column: "FitnessClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerEmployments");

            migrationBuilder.DropTable(
                name: "WorkerEmployments");

            migrationBuilder.DropTable(
                name: "FitnessClubs");
        }
    }
}
