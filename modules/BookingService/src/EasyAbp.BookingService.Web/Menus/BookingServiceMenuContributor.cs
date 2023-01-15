using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.BookingService.Localization;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.BookingService.Web.Menus;

public class BookingServiceMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        // context.Menu.AddItem(new ApplicationMenuItem(BookingServiceMenus.Prefix, displayName: "BookingService", "~/BookingService", icon: "fa fa-globe"));

        var l = context.GetLocalizer<BookingServiceResource>(); //Add main menu items.

        var bookingServiceMenuItem = context.Menu.Items.GetOrAdd(i => i.Name == BookingServiceCommonMenus.Prefix,
            () => new ApplicationMenuItem(BookingServiceCommonMenus.Prefix, l["Menu:BookingService"]));

        return Task.CompletedTask;
    }
}