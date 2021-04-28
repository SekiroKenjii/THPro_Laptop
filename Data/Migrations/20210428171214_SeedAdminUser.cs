using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "0343f548-fbf2-4be8-8b41-5c436b65cb5f", "Quản trị viên", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), new Guid("69bd714f-9576-45ba-b5b7-f00649be00de") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccessToken", "Address", "City", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "PublicId", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), 0, null, "KTX Khu B, Đại Học Quốc Gia TPHCM", "Thành phố Hồ Chí Minh", "d05ff96c-57af-43ac-a358-15d132302ba2", "Việt Nam", "trungthuongvo109@gmail.com", true, "Võ", "Trung Thường", false, null, "TRUNGTHUONGVO109@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEMtlejegAiWyz9MESGMCYlwA5u2zIZa/CDi0YEqa87PyKz23Jd/PVgge8oBaQHwyoQ==", "0375274267", true, "https://res.cloudinary.com/dglgzh0aj/image/upload/v1617718785/upload/img/user/IMG_20200910_134644_flstzn.jpg", "upload/img/user/IMG_20200910_134644_flstzn", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), new Guid("69bd714f-9576-45ba-b5b7-f00649be00de") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"));
        }
    }
}
