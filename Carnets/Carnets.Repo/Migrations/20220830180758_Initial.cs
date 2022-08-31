using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GympassTypes",
                columns: table => new
                {
                    GympassTypeId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GympassTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    ValidityPeriodInSeconds = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GympassTypes", x => x.GympassTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PermissionType = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Gympasses",
                columns: table => new
                {
                    GympassId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UserId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GympassTypeId = table.Column<string>(type: "character varying(30)", nullable: false),
                    FitnessClubId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ValidityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gympasses", x => x.GympassId);
                    table.ForeignKey(
                        name: "FK_Gympasses_GympassTypes_GympassTypeId",
                        column: x => x.GympassTypeId,
                        principalTable: "GympassTypes",
                        principalColumn: "GympassTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllowedEntriesPermissions",
                columns: table => new
                {
                    PermissionId = table.Column<string>(type: "character varying(30)", nullable: false),
                    AllowedEntries = table.Column<int>(type: "integer", nullable: false),
                    AllowedEntriesCooldown = table.Column<int>(type: "integer", nullable: false),
                    CooldownType = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedEntriesPermissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_AllowedEntriesPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignedPermissions",
                columns: table => new
                {
                    GympassTypeId = table.Column<string>(type: "character varying(30)", nullable: false),
                    PermissionId = table.Column<string>(type: "character varying(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedPermissions", x => new { x.GympassTypeId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_AssignedPermissions_GympassTypes_GympassTypeId",
                        column: x => x.GympassTypeId,
                        principalTable: "GympassTypes",
                        principalColumn: "GympassTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassPermissions",
                columns: table => new
                {
                    PermissionId = table.Column<string>(type: "character varying(30)", nullable: false),
                    PermissionName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassPermissions", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_ClassPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimePermissionEntries",
                columns: table => new
                {
                    PermissionId = table.Column<string>(type: "character varying(30)", nullable: false),
                    EnableEntryFrom = table.Column<byte>(type: "smallint", nullable: false),
                    EnableEntryTo = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePermissionEntries", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_TimePermissionEntries_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GympassId = table.Column<string>(type: "character varying(30)", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StripePaymentmethodId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Gympasses_GympassId",
                        column: x => x.GympassId,
                        principalTable: "Gympasses",
                        principalColumn: "GympassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedPermissions_PermissionId",
                table: "AssignedPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassPermissions_PermissionName",
                table: "ClassPermissions",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gympasses_GympassTypeId",
                table: "Gympasses",
                column: "GympassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_GympassId",
                table: "Subscriptions",
                column: "GympassId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StripeCustomerId",
                table: "Subscriptions",
                column: "StripeCustomerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedEntriesPermissions");

            migrationBuilder.DropTable(
                name: "AssignedPermissions");

            migrationBuilder.DropTable(
                name: "ClassPermissions");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TimePermissionEntries");

            migrationBuilder.DropTable(
                name: "Gympasses");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "GympassTypes");
        }
    }
}
