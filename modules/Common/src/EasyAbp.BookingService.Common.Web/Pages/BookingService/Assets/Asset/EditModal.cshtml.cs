using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.Assets;
using EasyAbp.BookingService.Assets.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.Assets.Asset;

public class EditModalModel : BookingServicePageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateEditAssetViewModel ViewModel { get; set; }

    private readonly IAssetAppService _service;

    public EditModalModel(IAssetAppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(Id);
        ViewModel = ObjectMapper.Map<AssetDto, CreateEditAssetViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditAssetViewModel, CreateUpdateAssetDto>(ViewModel);
        await _service.UpdateAsync(Id, dto);
        return NoContent();
    }
}