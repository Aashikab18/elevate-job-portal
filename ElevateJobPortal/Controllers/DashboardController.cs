using System.Security.Claims;
using ElevateJobPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevateJobPortal.Controllers;

[Authorize(Roles = "Company")]
public class DashboardController : Controller
{
    private readonly IApplicationService _applicationService;

    public DashboardController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var applications = await _applicationService.GetForCompanyAsync(userId);
        return View(applications);
    }
}