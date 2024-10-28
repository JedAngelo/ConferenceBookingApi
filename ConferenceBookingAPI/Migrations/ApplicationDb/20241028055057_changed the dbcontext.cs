using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class changedthedbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_AspNetUsers_InchargeUserId",
                table: "Conferences");

            migrationBuilder.DropTable(
                name: "ConferenceUsers");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_InchargeUserId",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "InchargeUserId",
                table: "Conferences");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ConferenceId",
                table: "AspNetUsers",
                column: "ConferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Conferences_ConferenceId",
                table: "AspNetUsers",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "ConferenceId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Conferences_ConferenceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ConferenceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "InchargeUserId",
                table: "Conferences",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_AspNetUsers_InchargeUserId",
                table: "Conferences",
                column: "InchargeUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
