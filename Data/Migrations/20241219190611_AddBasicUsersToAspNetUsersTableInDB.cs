using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace web_shop_app.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicUsersToAspNetUsersTableInDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var passwordHash = hasher.HashPassword(null, "Password12345");
            var user1Guid = Guid.NewGuid().ToString();
            var user2Guid = Guid.NewGuid().ToString();
            var roleGuid = Guid.NewGuid().ToString();


            migrationBuilder.Sql($@"
        INSERT INTO AspNetUsers (
            Id, UserName, NormalizedUserName, Email, EmailConfirmed,
            PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
            NormalizedEmail, PasswordHash, SecurityStamp, FirstName
        ) VALUES (
            '{user1Guid}', 'user1@user.com', 'USER1@USER.COM', 'user1@user.com', 0,
            0, 0, 0, 0, 'USER1@USER.COM', '{passwordHash}', NEWID(), 'User1'
        )
    ");

            migrationBuilder.Sql($@"
        INSERT INTO AspNetUsers (
            Id, UserName, NormalizedUserName, Email, EmailConfirmed,
            PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
            NormalizedEmail, PasswordHash, SecurityStamp, FirstName
        ) VALUES (
            '{user2Guid}', 'user2@user.com', 'USER2@USER.COM', 'user2@user.com', 0,
            0, 0, 0, 0, 'USER2@USER.COM', '{passwordHash}', NEWID(), 'User2'
        )
    ");

            // Insert role
            migrationBuilder.Sql($@"
        INSERT INTO AspNetRoles (Id, Name, NormalizedName)
        VALUES ('{roleGuid}', 'USER', 'USER')
    ");

            // Assign users to the role
            migrationBuilder.Sql($@"
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES ('{user1Guid}', '{roleGuid}')
    ");

            migrationBuilder.Sql($@"
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES ('{user2Guid}', '{roleGuid}')
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //
        }
    }
}
