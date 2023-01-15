using System.Threading.Tasks;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateEditAssetViewModel ViewModel { get; set; }

    private readonly IAssetAppService _service;

    public CreateModalModel(IAssetAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditAssetViewModel, CreateUpdateAssetDto>(ViewModel);
        await _service.CreateAsync(dto);
        return NoContent();
    }
}