﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace EasyAbp.BookingService.Pages;

public class IndexModel : BookingServicePageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
