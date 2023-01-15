using System.Threading.Tasks;
using EasyAbp.BookingService.AssetCategories;
using EasyAbp.BookingService.AssetCategories.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateAssetCategoryViewModel ViewModel { get; set; }

    private readonly IAssetCategoryAppService _service;

    public CreateModalModel(IAssetCategoryAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAssetCategoryViewModel, CreateAssetCategoryDto>(ViewModel);
        await _service.CreateAsync(dto);
        return NoContent();
    }
}