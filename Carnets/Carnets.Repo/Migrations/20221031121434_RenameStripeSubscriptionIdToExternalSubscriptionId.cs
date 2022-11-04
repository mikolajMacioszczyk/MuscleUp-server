using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carnets.Repo.Migrations
{
    public partial class RenameStripeSubscriptionIdToExternalSubscriptionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripeSubscriptionId",
                table: "Subscriptions",
                newName: "ExternalSubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalSubscriptionId",
                table: "Subscriptions",
                newName: "StripeSubscriptionId");
        }
    }
}
