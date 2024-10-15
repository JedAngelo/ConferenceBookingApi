using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class seperatedtimeanddate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "BookingStart",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "BookingEnd",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BookedDate",
                table: "Bookings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedDate",
                table: "Bookings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingStart",
                table: "Bookings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingEnd",
                table: "Bookings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }
    }
}
