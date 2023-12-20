using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantSubscriptionApp.Migrations
{
    public partial class SeedRoles : Migration
    {
        private string ManagerRoleId = Guid.NewGuid().ToString();
        private string UserRoleId = Guid.NewGuid().ToString();
        private string AdminRoleId = Guid.NewGuid().ToString();

        private string User1Id = Guid.NewGuid().ToString();
        private string User2Id = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedRolesSQL(migrationBuilder);

            SeedUser(migrationBuilder);

            SeedUserRoles(migrationBuilder);
        }

        private void SeedRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"INSERT INTO [dbo].[Roles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{AdminRoleId}', 'Administrator', 'ADMINISTRATOR', null);");
            migrationBuilder.Sql(@$"INSERT INTO [dbo].[Roles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{ManagerRoleId}', 'Manager', 'MANAGER', null);");
            migrationBuilder.Sql(@$"INSERT INTO [dbo].[Roles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
            VALUES ('{UserRoleId}', 'User', 'USER', null);");
        }

        private void SeedUser(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @$"INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], 
[Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
[PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Organisation]) 
VALUES 
(N'{User1Id}', N'Manager', N'Lastname', N'Manager@test.ca', N'TEST2@TEST.CA', 
N'test2@test.ca', N'TEST2@TEST.CA', 0, 
N'455a53bf-7ba0-4b97-b9c3-72b0c9191d3f', 
N'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', N'8e150555-a20d-4610-93ff-49c5af44f749', NULL, 0, 0, NULL, 1, 0, N'Master')");

            migrationBuilder.Sql(
                @$"INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], 
[Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
[PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Organisation]) 
VALUES 
(N'{User2Id}', N'Admin', N'Lastname', N'Admin@test.ca', N'TEST3@TEST.CA', 
N'test3@test.ca', N'TEST3@TEST.CA', 0, 
N'455a53bf-7ba0-4b97-b9c3-72b0c9191d3f', 
N'YUPAFWNGZI2UC5FOITC7PX5J7XZTAZAA', N'8e150555-a20d-4610-93ff-49c5af44f749', NULL, 0, 0, NULL, 1, 0, N'Master')");
        }

        private void SeedUserRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
        INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId])
        VALUES
           ('{User1Id}', '{ManagerRoleId}');
        INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId])
        VALUES
           ('{User1Id}', '{UserRoleId}');");

            migrationBuilder.Sql(@$"
        INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId])
        VALUES
           ('{User2Id}', '{AdminRoleId}');
        INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId])
        VALUES
           ('{User2Id}', '{ManagerRoleId}');");

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
