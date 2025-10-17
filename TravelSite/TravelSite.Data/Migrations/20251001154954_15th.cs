using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _15th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "BookingNotification",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
					RecipientId = table.Column<string>(type: "nvarchar(450)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BookingNotification", x => x.Id);
					table.ForeignKey(
						name: "FK_BookingNotification_AspNetUsers_RecipientId",
						column: x => x.RecipientId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.SetNull);
					table.ForeignKey(
						name: "FK_BookingNotification_AspNetUsers_SenderId",
						column: x => x.SenderId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
					table.ForeignKey(
						name: "FK_BookingNotification_Bookings_BookingId",
						column: x => x.BookingId,
						principalTable: "Bookings",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
				});

			migrationBuilder.CreateIndex(
				name: "IX_BookingNotification_BookingId",
				table: "BookingNotification",
				column: "BookingId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_BookingNotification_RecipientId",
				table: "BookingNotification",
				column: "RecipientId");

			migrationBuilder.CreateIndex(
				name: "IX_BookingNotification_SenderId",
				table: "BookingNotification",
				column: "SenderId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
