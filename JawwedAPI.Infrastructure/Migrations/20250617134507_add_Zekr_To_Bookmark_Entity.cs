using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Zekr_To_Bookmark_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookmarkType",
                table: "Bookmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZekrID",
                table: "Bookmarks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_ZekrID",
                table: "Bookmarks",
                column: "ZekrID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_Azkar_ZekrID",
                table: "Bookmarks",
                column: "ZekrID",
                principalTable: "Azkar",
                principalColumn: "ZekrID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_Azkar_ZekrID",
                table: "Bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_Bookmarks_ZekrID",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "BookmarkType",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "ZekrID",
                table: "Bookmarks");
        }
    }
}
