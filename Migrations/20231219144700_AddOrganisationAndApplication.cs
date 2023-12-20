using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantSubscriptionApp.Migrations
{
    public partial class AddOrganisationAndApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_Users_Organisation",
            //    table: "Users");

            migrationBuilder.DropColumn(
                name: "Organisation",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "TenantSubscriptions");

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TenantSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "TenantSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "TenantSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganisationId",
                table: "Users",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_ApplicationId",
                table: "TenantSubscriptions",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_OrganisationId",
                table: "TenantSubscriptions",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSubscriptions_Applications_ApplicationId",
                table: "TenantSubscriptions",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSubscriptions_Organisations_OrganisationId",
                table: "TenantSubscriptions",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSubscriptions_Applications_ApplicationId",
                table: "TenantSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantSubscriptions_Organisations_OrganisationId",
                table: "TenantSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganisationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TenantSubscriptions_ApplicationId",
                table: "TenantSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_TenantSubscriptions_OrganisationId",
                table: "TenantSubscriptions");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "TenantSubscriptions");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "TenantSubscriptions");

            migrationBuilder.AddColumn<string>(
                name: "Organisation",
                table: "Users",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TenantSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "TenantSubscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Organisation",
                table: "Users",
                column: "Organisation",
                unique: true);
        }
    }
}
