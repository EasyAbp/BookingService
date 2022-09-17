using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.BookingService.Web.Menus;

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
        context.Menu.AddItem(new ApplicationMenuItem(BookingServiceOrleansMenus.Prefix, displayName: "BookingService", "~/BookingService", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
