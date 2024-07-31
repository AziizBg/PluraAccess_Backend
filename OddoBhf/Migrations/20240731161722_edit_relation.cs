using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    /// <inheritdoc />
    public partial class edit_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_LicenceId",
                table: "Sessions");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LicenceId",
                table: "Sessions",
                column: "LicenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_LicenceId",
                table: "Sessions");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LicenceId",
                table: "Sessions",
                column: "LicenceId",
                unique: true,
                filter: "[LicenceId] IS NOT NULL");
        }
    }
}
