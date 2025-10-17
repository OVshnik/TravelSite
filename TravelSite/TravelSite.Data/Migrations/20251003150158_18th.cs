using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _18th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_Bookings_BookingId",
                table: "BookingNotification");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingId",
                table: "BookingNotification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_Bookings_BookingId",
                table: "BookingNotification",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_Bookings_BookingId",
                table: "BookingNotification");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookingId",
                table: "BookingNotification",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_Bookings_BookingId",
                table: "BookingNotification",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
