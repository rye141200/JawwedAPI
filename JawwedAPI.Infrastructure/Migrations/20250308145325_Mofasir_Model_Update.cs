using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mofasir_Model_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("38e70178-e001-4983-8c2a-42202da0fea5"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("449250d6-273b-4fb2-ab97-7700a8cddf89"));

            migrationBuilder.RenameColumn(
                name: "Languages",
                table: "Mofasirs",
                newName: "DeathYear");

            migrationBuilder.RenameColumn(
                name: "BookName",
                table: "Mofasirs",
                newName: "BookNameEnglish");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Mofasirs",
                newName: "BookNameArabic");

            migrationBuilder.AddColumn<string>(
                name: "AuthorNameArabic",
                table: "Mofasirs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorNameEnglish",
                table: "Mofasirs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BiographyArabic",
                table: "Mofasirs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BiographyEnglish",
                table: "Mofasirs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthYear",
                table: "Mofasirs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsArabic",
                table: "Mofasirs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsEnglish",
                table: "Mofasirs",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.DropColumn(
                name: "AuthorNameArabic",
                table: "Mofasirs");

            migrationBuilder.DropColumn(
                name: "AuthorNameEnglish",
                table: "Mofasirs");

            migrationBuilder.DropColumn(
                name: "BiographyArabic",
                table: "Mofasirs");

            migrationBuilder.DropColumn(
                name: "BiographyEnglish",
                table: "Mofasirs");

            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "Mofasirs");

            migrationBuilder.DropColumn(
                name: "SupportsArabic",
                table: "Mofasirs");

            migrationBuilder.DropColumn(
                name: "SupportsEnglish",
                table: "Mofasirs");

            migrationBuilder.RenameColumn(
                name: "DeathYear",
                table: "Mofasirs",
                newName: "Languages");

            migrationBuilder.RenameColumn(
                name: "BookNameEnglish",
                table: "Mofasirs",
                newName: "BookName");

            migrationBuilder.RenameColumn(
                name: "BookNameArabic",
                table: "Mofasirs",
                newName: "AuthorName");

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "UserId", "Email", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("38e70178-e001-4983-8c2a-42202da0fea5"), "thecityhunterhd@gmail.com", "Ahmad Mahfouz", 0 },
                    { new Guid("449250d6-273b-4fb2-ab97-7700a8cddf89"), "ahmad.mhfz1412@gmail.com", "Ahmad Mahfouz", 1 }
                });
        }
    }
}
