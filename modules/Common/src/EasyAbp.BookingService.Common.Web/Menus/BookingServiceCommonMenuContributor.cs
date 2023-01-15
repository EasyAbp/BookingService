using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.Localization;
using EasyAbp.BookingService.Permissions;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.BookingService.Web.Menus;

public class BookingServiceCommonMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<BookingServiceResource>(); //Add main menu items.

        var bookingServiceMenuItem = context.Menu.Items.GetOrAdd(i => i.Name == BookingServiceCommonMenus.Prefix,
            () => new ApplicationMenuItem(BookingServiceCommonMenus.Prefix, l["Menu:BookingService"]));

        if (await context.IsGrantedAsync(BookingServicePermissions.AssetCategory.Default))
        {
            bookingServiceMenuItem.AddItem(
                new ApplicationMenuItem(BookingServiceCommonMenus.AssetCategory, l["Menu:AssetCategory"],
                    "/BookingService/AssetCategories/AssetCategory")
            );
        }

        if (await context.IsGrantedAsync(BookingServicePermissions.AssetOccupancy.Default))
        {
            bookingServiceMenuItem.AddItem(
                new ApplicationMenuItem(BookingServiceCommonMenus.AssetOccupancy, l["Menu:AssetOccupancy"],
                    "/BookingService/AssetOccupancies/AssetOccupancy")
            );
        }

        if (await context.IsGrantedAsync(BookingServicePermissions.AssetPeriodScheme.Default))
        {
            bookingServiceMenuItem.AddItem(
                new ApplicationMenuItem(BookingServiceCommonMenus.AssetPeriodScheme, l["Menu:AssetPeriodScheme"],
                    "/BookingService/AssetPeriodSchemes/AssetPeriodScheme")
            );
        }

        if (await context.IsGrantedAsync(BookingServicePermissions.Asset.Default))
        {
            bookingServiceMenuItem.AddItem(
                new ApplicationMenuItem(BookingServiceCommonMenus.Asset, l["Menu:Asset"],
                    "/BookingService/Assets/Asset")
            );
        }

        if (await context.IsGrantedAsync(BookingServicePermissions.AssetSchedule.Default))
        {
            bookingServiceMenuItem.AddItem(
                new ApplicationMenuItem(BookingServiceCommonMenus.AssetSchedule, l["Menu:AssetSchedule"],
                    "/BookingService/AssetSchedules/AssetSchedule")
            );
        }

        if (await context.IsGrantedAsync(BookingServicePermissions.PeriodScheme.Default))
        {
            bookingServiceMenuItem.AddItem(
                new ApplicationMenuItem(BookingServiceCommonMenus.PeriodScheme, l["Menu:PeriodScheme"],
                    "/BookingService/PeriodSchemes/PeriodScheme")
            );
        }
    }
}