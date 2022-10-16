using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.PeriodScheme.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.PeriodScheme;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateEditPeriodSchemeViewModel ViewModel { get; set; }

    private readonly IPeriodSchemeAppService _service;

    public CreateModalModel(IPeriodSchemeAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditPeriodSchemeViewModel, CreatePeriodSchemeDto>(ViewModel);
        await _service.CreateAsync(dto);
        return NoContent();
    }
}