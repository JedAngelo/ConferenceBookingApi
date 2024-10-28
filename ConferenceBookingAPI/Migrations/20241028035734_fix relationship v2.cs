using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixrelationshipv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_ApplicationUser_InchargeUserId1",
                table: "Conferences");

            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceUser_ApplicationUser_UserId1",
                table: "ConferenceUser");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceUser_UserId1",
                table: "ConferenceUser");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_InchargeUserId1",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ConferenceUser");

            migrationBuilder.DropColumn(
                name: "InchargeUserId1",
                table: "Conferences");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ConferenceUser",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InchargeUserId",
                table: "Conferences",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUser_UserId",
                table: "ConferenceUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_InchargeUserId",
                table: "Conferences",
                column: "InchargeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_ApplicationUser_InchargeUserId",
                table: "Conferences",
                column: "InchargeUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceUser_ApplicationUser_UserId",
                table: "ConferenceUser",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_ApplicationUser_InchargeUserId",
                table: "Conferences");

            migrationBuilder.DropForeignKey(
                name: "FK_ConferenceUser_ApplicationUser_UserId",
                table: "ConferenceUser");

            migrationBuilder.DropIndex(
                name: "IX_ConferenceUser_UserId",
                table: "ConferenceUser");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_InchargeUserId",
                table: "Conferences");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ConferenceUser",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ConferenceUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InchargeUserId",
                table: "Conferences",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InchargeUserId1",
                table: "Conferences",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUser_UserId1",
                table: "ConferenceUser",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_InchargeUserId1",
                table: "Conferences",
                column: "InchargeUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_ApplicationUser_InchargeUserId1",
                table: "Conferences",
                column: "InchargeUserId1",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConferenceUser_ApplicationUser_UserId1",
                table: "ConferenceUser",
                column: "UserId1",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
