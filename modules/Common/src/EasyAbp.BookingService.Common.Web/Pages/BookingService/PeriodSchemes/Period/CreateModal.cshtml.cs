using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period;

public class CreateModalModel : BookingServicePageModel
{
    [BindProperty]
    public CreateEditPeriodViewModel ViewModel { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid PeriodSchemeId { get; set; }

    private readonly IPeriodSchemeAppService _service;

    public CreateModalModel(IPeriodSchemeAppService service)
    {
        _service = service;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditPeriodViewModel, CreateUpdatePeriodDto>(ViewModel);
        await _service.CreatePeriodAsync(PeriodSchemeId, dto);
        return NoContent();
    }
}