using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAbp.BookingService.Migrations
{
    public partial class AddedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssetCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssetDefinitionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultPeriodUsable = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MaxDaysInAdvance = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MaxTimespanInAdvance = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeInAdvance_MinDaysInAdvance = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MinTimespanInAdvance = table.Column<TimeSpan>(type: "time", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssetCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EasyAbpBookingServiceAssetCategories_EasyAbpBookingServiceAssetCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "EasyAbpBookingServiceAssetCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssetOccupancies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Asset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetDefinitionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    OccupierUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OccupierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssetOccupancies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssetPeriodSchemes",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssetPeriodSchemes", x => new { x.AssetId, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetDefinitionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultPeriodUsable = table.Column<int>(type: "int", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    TimeInAdvance_MaxDaysInAdvance = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MaxTimespanInAdvance = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeInAdvance_MinDaysInAdvance = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MinTimespanInAdvance = table.Column<TimeSpan>(type: "time", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssetSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodUsable = table.Column<int>(type: "int", nullable: false),
                    TimeInAdvance_MaxDaysInAdvance = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MaxTimespanInAdvance = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeInAdvance_MinDaysInAdvance = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvance_MinTimespanInAdvance = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssetSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServicePeriodSchemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServicePeriodSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Period",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Period", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Period_EasyAbpBookingServicePeriodSchemes_PeriodSchemeId",
                        column: x => x.PeriodSchemeId,
                        principalTable: "EasyAbpBookingServicePeriodSchemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetCategories_ParentId",
                table: "EasyAbpBookingServiceAssetCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_AssetId",
                table: "EasyAbpBookingServiceAssetOccupancies",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_AssetId_Date",
                table: "EasyAbpBookingServiceAssetOccupancies",
                columns: new[] { "AssetId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Period_PeriodSchemeId",
                table: "Period",
                column: "PeriodSchemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EasyAbpBookingServiceAssetCategories");

            migrationBuilder.DropTable(
                name: "EasyAbpBookingServiceAssetOccupancies");

            migrationBuilder.DropTable(
                name: "EasyAbpBookingServiceAssetPeriodSchemes");

            migrationBuilder.DropTable(
                name: "EasyAbpBookingServiceAssets");

            migrationBuilder.DropTable(
                name: "EasyAbpBookingServiceAssetSchedules");

            migrationBuilder.DropTable(
                name: "Period");

            migrationBuilder.DropTable(
                name: "EasyAbpBookingServicePeriodSchemes");
        }
    }
}
