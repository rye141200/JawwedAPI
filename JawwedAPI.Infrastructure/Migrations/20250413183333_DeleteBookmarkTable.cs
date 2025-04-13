using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBookmarkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("5b515abf-ec66-4d9e-aecc-645e0ccb9112"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("bd62576b-0410-42ce-9a3b-213c519fa08a"));

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "UserId", "Email", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("18e83293-3400-447c-b38b-e7e9c62bf220"), "ahmad.mhfz1412@gmail.com", "Ahmad Mahfouz", 1 },
                    { new Guid("b4f2b556-8789-4db3-b4b1-e0222f49a8e6"), "thecityhunterhd@gmail.com", "Ahmad Mahfouz", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("18e83293-3400-447c-b38b-e7e9c62bf220"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b4f2b556-8789-4db3-b4b1-e0222f49a8e6"));

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Verse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerseKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.BookmarkId);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "UserId", "Email", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("5b515abf-ec66-4d9e-aecc-645e0ccb9112"), "ahmad.mhfz1412@gmail.com", "Ahmad Mahfouz", 1 },
                    { new Guid("bd62576b-0410-42ce-9a3b-213c519fa08a"), "thecityhunterhd@gmail.com", "Ahmad Mahfouz", 0 }
                });
        }
    }
}
