using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateAssetScheduleViewModel ViewModel { get; set; }

    private readonly IAssetScheduleAppService _service;

    public CreateModalModel(IAssetScheduleAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAssetScheduleViewModel, CreateAssetScheduleDto>(ViewModel);
        await _service.CreateAsync(dto);
        return NoContent();
    }
}