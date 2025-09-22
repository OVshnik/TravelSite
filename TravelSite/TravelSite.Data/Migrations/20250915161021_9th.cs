using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _9th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookingNotification_BookingId",
                table: "BookingNotification");

            migrationBuilder.CreateIndex(
                name: "IX_BookingNotification_BookingId",
                table: "BookingNotification",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookingNotification_BookingId",
                table: "BookingNotification");

            migrationBuilder.CreateIndex(
                name: "IX_BookingNotification_BookingId",
                table: "BookingNotification",
                column: "BookingId",
                unique: true);
        }
    }
}
