using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class ChangeWorkerEmploymentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerEmployments_FitnessClubs_FitnessClubId",
                table: "WorkerEmployments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerEmployments",
                table: "WorkerEmployments");

            migrationBuilder.AlterColumn<string>(
                name: "FitnessClubId",
                table: "WorkerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WorkerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "WorkerEmploymentId",
                table: "WorkerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerEmployments",
                table: "WorkerEmployments",
                column: "WorkerEmploymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerEmployments_FitnessClubs_FitnessClubId",
                table: "WorkerEmployments",
                column: "FitnessClubId",
                principalTable: "FitnessClubs",
                principalColumn: "FitnessClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerEmployments_FitnessClubs_FitnessClubId",
                table: "WorkerEmployments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerEmployments",
                table: "WorkerEmployments");

            migrationBuilder.DropColumn(
                name: "WorkerEmploymentId",
                table: "WorkerEmployments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WorkerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FitnessClubId",
                table: "WorkerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerEmployments",
                table: "WorkerEmployments",
                columns: new[] { "UserId", "FitnessClubId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerEmployments_FitnessClubs_FitnessClubId",
                table: "WorkerEmployments",
                column: "FitnessClubId",
                principalTable: "FitnessClubs",
                principalColumn: "FitnessClubId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
