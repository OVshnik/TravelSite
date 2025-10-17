using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class _16th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<bool>(
				name: "Delivered",
				table: "BookingNotification",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
