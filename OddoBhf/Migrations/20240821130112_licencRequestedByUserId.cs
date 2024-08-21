using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    /// <inheritdoc />
    public partial class licencRequestedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBeingRequested",
                table: "Licences");

            migrationBuilder.AddColumn<int>(
                name: "BookedByUserId",
                table: "Licences",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedByUserId",
                table: "Licences");

            migrationBuilder.AddColumn<bool>(
                name: "IsBeingRequested",
                table: "Licences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
