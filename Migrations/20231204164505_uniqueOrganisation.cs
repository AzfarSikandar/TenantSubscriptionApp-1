using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantSubscriptionApp.Migrations
{
    public partial class uniqueOrganisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Organisation",
                table: "Users",
                column: "Organisation",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Organisation",
                table: "Users");
        }
    }
}
