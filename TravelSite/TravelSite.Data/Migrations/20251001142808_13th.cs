using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _13th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_AspNetUsers_RecipientId",
                table: "BookingNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_AspNetUsers_SenderId",
                table: "BookingNotification");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "BookingNotification",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "BookingNotification",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_AspNetUsers_RecipientId",
                table: "BookingNotification",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

			migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_AspNetUsers_SenderId",
                table: "BookingNotification",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_AspNetUsers_RecipientId",
                table: "BookingNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingNotification_AspNetUsers_SenderId",
                table: "BookingNotification");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "BookingNotification",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "BookingNotification",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_AspNetUsers_RecipientId",
                table: "BookingNotification",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNotification_AspNetUsers_SenderId",
                table: "BookingNotification",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
