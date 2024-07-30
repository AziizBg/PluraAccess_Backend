using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Licences_LicenceId1",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_LicenceId1",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "LicenceId1",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Licences");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicenceId1",
                table: "Sessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Licences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LicenceId1",
                table: "Sessions",
                column: "LicenceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Licences_LicenceId1",
                table: "Sessions",
                column: "LicenceId1",
                principalTable: "Licences",
                principalColumn: "Id");
        }
    }
}
