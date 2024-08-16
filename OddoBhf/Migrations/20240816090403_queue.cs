using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OddoBhf.Migrations
{
    public partial class queue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Create the Queue table
            migrationBuilder.CreateTable(
                name: "Queue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Queue_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Queue_UserId",
                table: "Queue",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Queue");

            // Uncomment the following line if you had created the Users table in this migration
            // migrationBuilder.DropTable(name: "Users");
        }
    }
}