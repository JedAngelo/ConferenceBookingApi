using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConferenceBookings",
                columns: table => new
                {
                    ConferenceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Meeting = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceBookings", x => x.ConferenceID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConferenceBookings");
        }
    }
}
