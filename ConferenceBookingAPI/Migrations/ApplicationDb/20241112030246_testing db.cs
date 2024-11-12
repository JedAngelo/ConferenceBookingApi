using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class testingdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StatusId",
                table: "Bookings",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Statuses_StatusId",
                table: "Bookings",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Statuses_StatusId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StatusId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
