using System.Threading.Tasks;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetOccupancies.AssetOccupancy;

public class IndexModel : BookingServicePageModel
{
    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}