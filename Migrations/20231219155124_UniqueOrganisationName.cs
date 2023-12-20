using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantSubscriptionApp.Migrations
{
    public partial class UniqueOrganisationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
       

            

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organisations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_Name",
                table: "Organisations",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organisations_Name",
                table: "Organisations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organisations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
