using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    /// <inheritdoc />
    public partial class relationsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licences_Sessions_CurrentSessionId",
                table: "Licences");

            migrationBuilder.DropIndex(
                name: "IX_Licences_CurrentSessionId",
                table: "Licences");

            migrationBuilder.DropColumn(
                name: "CurrentSessionId",
                table: "Licences");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LicenceId",
                table: "Sessions",
                column: "LicenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Licences_LicenceId",
                table: "Sessions",
                column: "LicenceId",
                principalTable: "Licences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Licences_LicenceId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_LicenceId",
                table: "Sessions");

            migrationBuilder.AddColumn<int>(
                name: "CurrentSessionId",
                table: "Licences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Licences_CurrentSessionId",
                table: "Licences",
                column: "CurrentSessionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Licences_Sessions_CurrentSessionId",
                table: "Licences",
                column: "CurrentSessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
