using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAbp.BookingService.Migrations
{
    public partial class AddedVolume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "EasyAbpBookingServiceAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "EasyAbpBookingServiceAssetOccupancies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Volume",
                table: "EasyAbpBookingServiceAssets");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "EasyAbpBookingServiceAssetOccupancies");
        }
    }
}
