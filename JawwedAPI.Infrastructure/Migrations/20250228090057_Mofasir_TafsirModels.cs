using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mofasir_TafsirModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mofasirs",
                columns: table => new
                {
                    MofasirID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mofasirs", x => x.MofasirID);
                });

            migrationBuilder.CreateTable(
                name: "Tafsirs",
                columns: table => new
                {
                    TafsirID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterVerseID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MofasirID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tafsirs", x => x.TafsirID);
                    table.ForeignKey(
                        name: "FK_Tafsirs_Mofasirs_MofasirID",
                        column: x => x.MofasirID,
                        principalTable: "Mofasirs",
                        principalColumn: "MofasirID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tafsirs_MofasirID",
                table: "Tafsirs",
                column: "MofasirID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tafsirs");

            migrationBuilder.DropTable(
                name: "Mofasirs");
        }
    }
}
