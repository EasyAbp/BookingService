using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAbp.BookingService.Blazor.Server.Host.Migrations
{
    /// <inheritdoc />
    public partial class UpgradedToAbp71 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityVersion",
                table: "AbpUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityVersion",
                table: "AbpTenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityVersion",
                table: "AbpRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityVersion",
                table: "AbpOrganizationUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AbpFeatureGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatureGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ParentName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsVisibleToClients = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailableToHost = table.Column<bool>(type: "bit", nullable: false),
                    AllowedProviders = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ValueType = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ParentName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    MultiTenancySide = table.Column<byte>(type: "tinyint", nullable: false),
                    Providers = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    StateCheckers = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EasyAbpBookingServiceAssetCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssetDefinitionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultPeriodUsable = table.Column<int>(type: "int", nullable: true),
                    TimeInAdvanceMaxDaysInAdvance = table.Column<int>(name: "TimeInAdvance_MaxDaysInAdvance", type: "int", nullable: true),
                    TimeInAdvanceMaxTimespanInAdvance = table.Column<TimeSpan>(name: "TimeInAdvance_MaxTimespanInAdvance", type: "time", nullable: true),
                    TimeInAdvanceMinDaysInAdvance = table.Column<int>(name: "TimeInAdvance_MinDaysInAdvance", type: "int", nullable: true),
                    TimeInAdvanceMinTimespanInAdvance = table.Column<TimeSpan>(name: "TimeInAdvance_MinTimespanInAdvance", type: "time", nullable: true),
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
                    Volume = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_EasyAbpBookingServiceAssetPeriodSchemes", x => new { x.Date, x.AssetId });
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
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    TimeInAdvanceMaxDaysInAdvance = table.Column<int>(name: "TimeInAdvance_MaxDaysInAdvance", type: "int", nullable: true),
                    TimeInAdvanceMaxTimespanInAdvance = table.Column<TimeSpan>(name: "TimeInAdvance_MaxTimespanInAdvance", type: "time", nullable: true),
                    TimeInAdvanceMinDaysInAdvance = table.Column<int>(name: "TimeInAdvance_MinDaysInAdvance", type: "int", nullable: true),
                    TimeInAdvanceMinTimespanInAdvance = table.Column<TimeSpan>(name: "TimeInAdvance_MinTimespanInAdvance", type: "time", nullable: true),
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
                    TimeInAdvanceMaxDaysInAdvance = table.Column<int>(name: "TimeInAdvance_MaxDaysInAdvance", type: "int", nullable: true),
                    TimeInAdvanceMaxTimespanInAdvance = table.Column<TimeSpan>(name: "TimeInAdvance_MaxTimespanInAdvance", type: "time", nullable: true),
                    TimeInAdvanceMinDaysInAdvance = table.Column<int>(name: "TimeInAdvance_MinDaysInAdvance", type: "int", nullable: true),
                    TimeInAdvanceMinTimespanInAdvance = table.Column<TimeSpan>(name: "TimeInAdvance_MinTimespanInAdvance", type: "time", nullable: true),
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
                name: "EasyAbpBookingServicePeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    PeriodSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasyAbpBookingServicePeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EasyAbpBookingServicePeriods_EasyAbpBookingServicePeriodSchemes_PeriodSchemeId",
                        column: x => x.PeriodSchemeId,
                        principalTable: "EasyAbpBookingServicePeriodSchemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatureGroups_Name",
                table: "AbpFeatureGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_GroupName",
                table: "AbpFeatures",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_Name",
                table: "AbpFeatures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGroups_Name",
                table: "AbpPermissionGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_GroupName",
                table: "AbpPermissions",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_Name",
                table: "AbpPermissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetCategories_ParentId",
                table: "EasyAbpBookingServiceAssetCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_Date_AssetId_StartingTime_Duration",
                table: "EasyAbpBookingServiceAssetOccupancies",
                columns: new[] { "Date", "AssetId", "StartingTime", "Duration" });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetOccupancies_Date_OccupierUserId",
                table: "EasyAbpBookingServiceAssetOccupancies",
                columns: new[] { "Date", "OccupierUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServiceAssetSchedules_Date_AssetId_PeriodSchemeId",
                table: "EasyAbpBookingServiceAssetSchedules",
                columns: new[] { "Date", "AssetId", "PeriodSchemeId" });

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpBookingServicePeriods_PeriodSchemeId",
                table: "EasyAbpBookingServicePeriods",
                column: "PeriodSchemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpFeatureGroups");

            migrationBuilder.DropTable(
                name: "AbpFeatures");

            migrationBuilder.DropTable(
                name: "AbpPermissionGroups");

            migrationBuilder.DropTable(
                name: "AbpPermissions");

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
                name: "EasyAbpBookingServicePeriods");

            migrationBuilder.DropTable(
                name: "EasyAbpBookingServicePeriodSchemes");

            migrationBuilder.DropColumn(
                name: "EntityVersion",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "EntityVersion",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "EntityVersion",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "EntityVersion",
                table: "AbpOrganizationUnits");
        }
    }
}
