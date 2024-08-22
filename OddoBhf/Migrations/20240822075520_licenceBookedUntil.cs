using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    /// <inheritdoc />
    public partial class licenceBookedUntil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookedUntil",
                table: "Licences",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedUntil",
                table: "Licences");
        }
    }
}
