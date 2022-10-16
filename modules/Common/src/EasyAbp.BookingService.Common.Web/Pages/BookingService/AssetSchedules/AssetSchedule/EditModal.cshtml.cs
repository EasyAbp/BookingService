using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.AssetSchedules;
using EasyAbp.BookingService.AssetSchedules.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.AssetSchedules.AssetSchedule;

public class EditModalModel : BookingServicePageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditAssetScheduleViewModel ViewModel { get; set; }

    private readonly IAssetScheduleAppService _service;

    public EditModalModel(IAssetScheduleAppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(Id);
        ViewModel = ObjectMapper.Map<AssetScheduleDto, EditAssetScheduleViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditAssetScheduleViewModel, UpdateAssetScheduleDto>(ViewModel);
        await _service.UpdateAsync(Id, dto);
        return NoContent();
    }
}