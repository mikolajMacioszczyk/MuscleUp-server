using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessClubs.Repo.Migrations
{
    public partial class EmploymentBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerEmployments_FitnessClubs_FitnessClubId",
                table: "TrainerEmployments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerEmployments",
                table: "TrainerEmployments");

            migrationBuilder.RenameColumn(
                name: "WorkerEmploymentId",
                table: "WorkerEmployments",
                newName: "EmploymentId");

            migrationBuilder.AlterColumn<string>(
                name: "FitnessClubId",
                table: "TrainerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentId",
                table: "TrainerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EmployedFrom",
                table: "TrainerEmployments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EmployedTo",
                table: "TrainerEmployments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerEmployments",
                table: "TrainerEmployments",
                column: "EmploymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerEmployments_FitnessClubs_FitnessClubId",
                table: "TrainerEmployments",
                column: "FitnessClubId",
                principalTable: "FitnessClubs",
                principalColumn: "FitnessClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerEmployments_FitnessClubs_FitnessClubId",
                table: "TrainerEmployments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerEmployments",
                table: "TrainerEmployments");

            migrationBuilder.DropColumn(
                name: "EmploymentId",
                table: "TrainerEmployments");

            migrationBuilder.DropColumn(
                name: "EmployedFrom",
                table: "TrainerEmployments");

            migrationBuilder.DropColumn(
                name: "EmployedTo",
                table: "TrainerEmployments");

            migrationBuilder.RenameColumn(
                name: "EmploymentId",
                table: "WorkerEmployments",
                newName: "WorkerEmploymentId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainerEmployments",
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
                table: "TrainerEmployments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerEmployments",
                table: "TrainerEmployments",
                columns: new[] { "UserId", "FitnessClubId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerEmployments_FitnessClubs_FitnessClubId",
                table: "TrainerEmployments",
                column: "FitnessClubId",
                principalTable: "FitnessClubs",
                principalColumn: "FitnessClubId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
