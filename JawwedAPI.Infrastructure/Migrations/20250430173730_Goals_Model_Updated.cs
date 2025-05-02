﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JawwedAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Goals_Model_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualPagesRead",
                table: "Goals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualPagesRead",
                table: "Goals");
        }
    }
}
