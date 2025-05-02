using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Applicationuser_Model_timeramider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ReminderTime",
                table: "Goals",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Goals");
        }
    }
}
