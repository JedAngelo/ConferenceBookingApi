using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class fixrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conferences",
                columns: table => new
                {
                    ConferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConferenceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InchargeUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conferences", x => x.ConferenceId);
                    table.ForeignKey(
                        name: "FK_Conferences_AspNetUsers_InchargeUserId",
                        column: x => x.InchargeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BookingStart = table.Column<TimeOnly>(type: "time", nullable: false),
                    BookingEnd = table.Column<TimeOnly>(type: "time", nullable: false),
                    Organizer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedAttendees = table.Column<int>(type: "int", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConferenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceUsers",
                columns: table => new
                {
                    ConferenceUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConferenceId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceUsers", x => x.ConferenceUserId);
                    table.ForeignKey(
                        name: "FK_ConferenceUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConferenceUsers_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ConferenceId",
                table: "Booking",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_InchargeUserId",
                table: "Conferences",
                column: "InchargeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUsers_ConferenceId",
                table: "ConferenceUsers",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUsers_UserId",
                table: "ConferenceUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "ConferenceUsers");

            migrationBuilder.DropTable(
                name: "Conferences");
        }
    }
}
