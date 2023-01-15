using System.Threading.Tasks;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateAssetPeriodSchemeViewModel ViewModel { get; set; }

    private readonly IAssetPeriodSchemeAppService _service;

    public CreateModalModel(IAssetPeriodSchemeAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAssetPeriodSchemeViewModel, CreateAssetPeriodSchemeDto>(ViewModel);
        await _service.CreateAsync(dto);
        return NoContent();
    }
}