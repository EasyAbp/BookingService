using System.Threading.Tasks;

namespace EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset;

public class IndexModel : BookingServicePageModel
{
    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}
