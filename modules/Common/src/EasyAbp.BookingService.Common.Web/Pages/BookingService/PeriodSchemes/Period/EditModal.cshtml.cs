using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.BookingService.PeriodSchemes;
using EasyAbp.BookingService.PeriodSchemes.Dtos;
using EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.BookingService.Web.Pages.BookingService.PeriodSchemes.Period;

public class EditModalModel : BookingServicePageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid PeriodSchemeId { get; set; }

    [BindProperty]
    public CreateEditPeriodViewModel ViewModel { get; set; }

    private readonly IPeriodSchemeAppService _service;

    public EditModalModel(IPeriodSchemeAppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(PeriodSchemeId);
        ViewModel = ObjectMapper.Map<PeriodDto, CreateEditPeriodViewModel>(dto.Periods.Single(x => x.Id == Id));
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditPeriodViewModel, CreateUpdatePeriodDto>(ViewModel);
        await _service.UpdatePeriodAsync(PeriodSchemeId, Id, dto);
        return NoContent();
    }
}