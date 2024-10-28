using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixrelionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_ApplicationUser_ApplicationUserId",
                table: "Conferences");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Conferences",
                newName: "InchargeUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Conferences_ApplicationUserId",
                table: "Conferences",
                newName: "IX_Conferences_InchargeUserId1");

            migrationBuilder.CreateTable(
                name: "ConferenceUser",
                columns: table => new
                {
                    ConferenceUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConferenceId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceUser", x => x.ConferenceUserId);
                    table.ForeignKey(
                        name: "FK_ConferenceUser_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConferenceUser_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUser_ConferenceId",
                table: "ConferenceUser",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUser_UserId",
                table: "ConferenceUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_ApplicationUser_InchargeUserId1",
                table: "Conferences",
                column: "InchargeUserId1",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_ApplicationUser_InchargeUserId1",
                table: "Conferences");

            migrationBuilder.DropTable(
                name: "ConferenceUser");

            migrationBuilder.RenameColumn(
                name: "InchargeUserId1",
                table: "Conferences",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Conferences_InchargeUserId1",
                table: "Conferences",
                newName: "IX_Conferences_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_ApplicationUser_ApplicationUserId",
                table: "Conferences",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
