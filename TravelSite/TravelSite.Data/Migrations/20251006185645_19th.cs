using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _19th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_TravelDatesId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TravelDatesId",
                table: "Bookings",
                column: "TravelDatesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_TravelDatesId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TravelDatesId",
                table: "Bookings",
                column: "TravelDatesId",
                unique: true);
        }
    }
}
