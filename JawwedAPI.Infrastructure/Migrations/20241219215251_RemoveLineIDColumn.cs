using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLineIDColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First drop the foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Verses_Lines_LineID",
                table: "Verses");

            // Then drop the index
            migrationBuilder.DropIndex(
                name: "IX_Verses_LineID", // Note: Changed from ChapterID to LineID
                table: "Verses");

            // Finally drop the column
            migrationBuilder.DropColumn(
                name: "LineID",
                table: "Verses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineID",
                table: "Verses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verses_LineID",
                table: "Verses",
                column: "LineID");

            migrationBuilder.AddForeignKey(
                name: "FK_Verses_Lines_LineID",
                table: "Verses",
                column: "LineID",
                principalTable: "Lines",
                principalColumn: "LineID");
        }
    }
}
