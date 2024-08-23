using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    /// <inheritdoc />
    public partial class user_bookedLicenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookedLicenceId",
                table: "Users",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedLicenceId",
                table: "Users");
        }
    }
}
