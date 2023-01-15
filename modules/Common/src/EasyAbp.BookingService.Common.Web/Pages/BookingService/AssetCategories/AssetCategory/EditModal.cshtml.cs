using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetCategories.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory;

public class EditModalModel : BookingServicePageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditAssetCategoryViewModel ViewModel { get; set; }

    private readonly IAssetCategoryAppService _service;

    public EditModalModel(IAssetCategoryAppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(Id);
        ViewModel = ObjectMapper.Map<AssetCategoryDto, EditAssetCategoryViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditAssetCategoryViewModel, UpdateAssetCategoryDto>(ViewModel);
        await _service.UpdateAsync(Id, dto);
        return NoContent();
    }
}