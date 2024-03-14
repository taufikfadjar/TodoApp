using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Model.Migrations
{
    /// <inheritdoc />
    public partial class seedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8c92032e-75bd-42cb-b2db-9a43e3459390", "83d42059-657e-4064-aa06-c29b417342dd", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f53f4178-8849-47f8-8b02-f871d36e8e05", 0, "8b84ced3-6695-4f2f-af2d-1b3c263ebd36", "taufikfadjar@live.com", false, "Taufik Fadjar", false, null, "TAUFIKFADJAR@LIVE.COM", "TAUFIKFADJAR", "AQAAAAEAACcQAAAAEDfJR6o+UHyXoQIUvFuoFENmy87KvofGXvAMWDgiBIyv8uSEVaqxNy0aCHnrLbNmjw==", null, false, "cba2d46c-85ca-466e-8043-3ae9537075b8", false, "taufikfadjar" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "8c92032e-75bd-42cb-b2db-9a43e3459390", "f53f4178-8849-47f8-8b02-f871d36e8e05" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8c92032e-75bd-42cb-b2db-9a43e3459390", "f53f4178-8849-47f8-8b02-f871d36e8e05" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c92032e-75bd-42cb-b2db-9a43e3459390");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f53f4178-8849-47f8-8b02-f871d36e8e05");
        }
    }
}
