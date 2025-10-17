using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _11th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookingNotification",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingNotification_UserId",
                table: "BookingNotification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_AspNetUsers_UserId",
                table: "BookingNotification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_AspNetUsers_UserId",
                table: "BookingNotification");

            migrationBuilder.DropIndex(
                name: "IX_BookingNotification_UserId",
                table: "BookingNotification");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookingNotification");
        }
    }
}
