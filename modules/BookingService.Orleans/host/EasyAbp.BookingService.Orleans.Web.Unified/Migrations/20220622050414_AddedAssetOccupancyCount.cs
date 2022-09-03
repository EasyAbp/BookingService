using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAbp.BookingService.Migrations
{
    public partial class AddedAssetOccupancyCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpBookingServiceAssetPeriodSchemes",
                table: "EasyAbpBookingServiceAssetPeriodSchemes");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_AssetId",
                table: "EasyAbpBookingServiceAssetOccupancies");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_AssetId_Date",
                table: "EasyAbpBookingServiceAssetOccupancies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpBookingServiceAssetPeriodSchemes",
                table: "EasyAbpBookingServiceAssetPeriodSchemes",
                columns: new[] { "Date", "AssetId" });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssetOccupancyCounts",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Asset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssetOccupancyCounts", x => new { x.Date, x.AssetId, x.StartingTime, x.Duration });
                });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetSchedules_Date_AssetId_PeriodSchemeId",
                table: "EasyAbpBookingServiceAssetSchedules",
                columns: new[] { "Date", "AssetId", "PeriodSchemeId" });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_Date_AssetId_StartingTime_Duration",
                table: "EasyAbpBookingServiceAssetOccupancies",
                columns: new[] { "Date", "AssetId", "StartingTime", "Duration" });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_Date_OccupierUserId",
                table: "EasyAbpBookingServiceAssetOccupancies",
                columns: new[] { "Date", "OccupierUserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EasyAbpBookingServiceAssetOccupancyCounts");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpBookingServiceAssetSchedules_Date_AssetId_PeriodSchemeId",
                table: "EasyAbpBookingServiceAssetSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpBookingServiceAssetPeriodSchemes",
                table: "EasyAbpBookingServiceAssetPeriodSchemes");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_Date_AssetId_StartingTime_Duration",
                table: "EasyAbpBookingServiceAssetOccupancies");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_Date_OccupierUserId",
                table: "EasyAbpBookingServiceAssetOccupancies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpBookingServiceAssetPeriodSchemes",
                table: "EasyAbpBookingServiceAssetPeriodSchemes",
                columns: new[] { "AssetId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_AssetId",
                table: "EasyAbpBookingServiceAssetOccupancies",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_AssetId_Date",
                table: "EasyAbpBookingServiceAssetOccupancies",
                columns: new[] { "AssetId", "Date" });
        }
    }
}
