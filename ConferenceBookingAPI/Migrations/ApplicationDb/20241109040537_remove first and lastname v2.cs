﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class removefirstandlastnamev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecurringEndDate",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringEndDate",
                table: "Bookings");
        }
    }
}