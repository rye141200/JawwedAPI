using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmailIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b649f808-b43b-4ed2-806e-a018221b1e19"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("e2b4972d-79c6-424c-867d-b68e405c5179"));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "UserId", "Email", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("38e70178-e001-4983-8c2a-42202da0fea5"), "thecityhunterhd@gmail.com", "Ahmad Mahfouz", 0 },
                    { new Guid("449250d6-273b-4fb2-ab97-7700a8cddf89"), "ahmad.mhfz1412@gmail.com", "Ahmad Mahfouz", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_Email",
                table: "ApplicationUsers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_Email",
                table: "ApplicationUsers");

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("38e70178-e001-4983-8c2a-42202da0fea5"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("449250d6-273b-4fb2-ab97-7700a8cddf89"));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "UserId", "Email", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("b649f808-b43b-4ed2-806e-a018221b1e19"), "thecityhunterhd@gmail.com", "Ahmad Mahfouz", 0 },
                    { new Guid("e2b4972d-79c6-424c-867d-b68e405c5179"), "ahmad.mhfz1412@gmail.com", "Ahmad Mahfouz", 1 }
                });
        }
    }
}
