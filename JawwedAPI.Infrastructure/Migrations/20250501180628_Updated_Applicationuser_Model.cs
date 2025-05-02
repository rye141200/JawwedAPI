using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Applicationuser_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<bool>(
                name: "EnableNotifications",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("18e83293-3400-447c-b38b-e7e9c62bf220"),
                columns: new[] { "DeviceToken", "EnableNotifications" },
                values: new object[] { "", false }
            );

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b4f2b556-8789-4db3-b4b1-e0222f49a8e6"),
                columns: new[] { "DeviceToken", "EnableNotifications" },
                values: new object[] { "", false }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DeviceToken", table: "ApplicationUsers");

            migrationBuilder.DropColumn(name: "EnableNotifications", table: "ApplicationUsers");
        }
    }
}
