using System;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.PeriodScheme.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.PeriodScheme;

public class EditModalModel : BookingServicePageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateEditPeriodSchemeViewModel ViewModel { get; set; }

    private readonly IPeriodSchemeAppService _service;

    public EditModalModel(IPeriodSchemeAppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(Id);
        ViewModel = ObjectMapper.Map<PeriodSchemeDto, CreateEditPeriodSchemeViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditPeriodSchemeViewModel, UpdatePeriodSchemeDto>(ViewModel);
        await _service.UpdateAsync(Id, dto);
        return NoContent();
    }
}