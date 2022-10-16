using System.Threading.Tasks;
using EasyAbp.BookingService.AssetOccupancies;
using EasyAbp.BookingService.AssetOccupancies.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetOccupancies.AssetOccupancy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetOccupancies.AssetOccupancy;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateAssetOccupancyViewModel ViewModel { get; set; }

    private readonly IAssetOccupancyAppService _service;

    public CreateModalModel(IAssetOccupancyAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAssetOccupancyViewModel, CreateAssetOccupancyDto>(ViewModel);
        await _service.CreateAsync(dto);
        return NoContent();
    }
}