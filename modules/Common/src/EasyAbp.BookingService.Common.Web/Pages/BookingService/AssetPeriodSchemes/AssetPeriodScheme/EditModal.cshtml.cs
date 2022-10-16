using System.Threading.Tasks;
using EasyAbp.BookingService.AssetPeriodSchemes;
using EasyAbp.BookingService.AssetPeriodSchemes.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetPeriodSchemes.AssetPeriodScheme;

public class EditModalModel : BookingServicePageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public AssetPeriodSchemeKey Id { get; set; }

    [BindProperty]
    public EditAssetPeriodSchemeViewModel ViewModel { get; set; }

    private readonly IAssetPeriodSchemeAppService _service;

    public EditModalModel(IAssetPeriodSchemeAppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(Id);
        ViewModel = ObjectMapper.Map<AssetPeriodSchemeDto, EditAssetPeriodSchemeViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditAssetPeriodSchemeViewModel, UpdateAssetPeriodSchemeDto>(ViewModel);
        await _service.UpdateAsync(Id, dto);
        return NoContent();
    }
}