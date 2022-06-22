using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAbp.BookingService.Migrations
{
    public partial class ConfiguredPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Period_EasyAbpBookingServicePeriodSchemes_PeriodSchemeId",
                table: "Period");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Period",
                table: "Period");

            migrationBuilder.RenameTable(
                name: "Period",
                newName: "EasyAbpBookingServicePeriods");

            migrationBuilder.RenameIndex(
                name: "IX_Period_PeriodSchemeId",
                table: "EasyAbpBookingServicePeriods",
                newName: "IX_EasyAbpBookingServicePeriods_PeriodSchemeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpBookingServicePeriods",
                table: "EasyAbpBookingServicePeriods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EasyAbpBookingServicePeriods_EasyAbpBookingServicePeriodSchemes_PeriodSchemeId",
                table: "EasyAbpBookingServicePeriods",
                column: "PeriodSchemeId",
                principalTable: "EasyAbpBookingServicePeriodSchemes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EasyAbpBookingServicePeriods_EasyAbpBookingServicePeriodSchemes_PeriodSchemeId",
                table: "EasyAbpBookingServicePeriods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpBookingServicePeriods",
                table: "EasyAbpBookingServicePeriods");

            migrationBuilder.RenameTable(
                name: "EasyAbpBookingServicePeriods",
                newName: "Period");

            migrationBuilder.RenameIndex(
                name: "IX_EasyAbpBookingServicePeriods_PeriodSchemeId",
                table: "Period",
                newName: "IX_Period_PeriodSchemeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Period",
                table: "Period",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Period_EasyAbpBookingServicePeriodSchemes_PeriodSchemeId",
                table: "Period",
                column: "PeriodSchemeId",
                principalTable: "EasyAbpBookingServicePeriodSchemes",
                principalColumn: "Id");
        }
    }
}
