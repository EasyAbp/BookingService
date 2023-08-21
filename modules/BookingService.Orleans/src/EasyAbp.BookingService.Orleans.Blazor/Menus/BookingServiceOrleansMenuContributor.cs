using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.BookingService.Blazor.Menus;

public class BookingServiceOrleansMenuContributor : IMenuContributor
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
        context.Menu.GetAdministration().AddItem(new ApplicationMenuItem(BookingServiceOrleansMenus.Prefix,
            displayName: "BookingService", "/BookingService", icon: "fa fa-book-user"));

        return Task.CompletedTask;
    }
}