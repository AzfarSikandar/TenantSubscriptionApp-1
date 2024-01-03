using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantSubscriptionApp.Migrations
{
    public partial class SeedApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedApplicationtionsSQL(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        private void SeedApplicationtionsSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Applications] ([Name]) VALUES (N'ERP')");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Applications] ([Name]) VALUES (N'HRMS')");
        }
    }
}
