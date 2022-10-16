using System.ComponentModel.DataAnnotations;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetCategories.AssetCategory.ViewModels;

public class CreateAssetCategoryViewModel : EditAssetCategoryViewModel
{
    [Display(Name = "AssetCategoryAssetDefinitionName")]
    public string AssetDefinitionName { get; set; }
}