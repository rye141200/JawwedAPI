using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "UserId", "Email", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("b649f808-b43b-4ed2-806e-a018221b1e19"), "thecityhunterhd@gmail.com", "Ahmad Mahfouz", 0 },
                    { new Guid("e2b4972d-79c6-424c-867d-b68e405c5179"), "ahmad.mhfz1412@gmail.com", "Ahmad Mahfouz", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUsers");
        }
    }
}
