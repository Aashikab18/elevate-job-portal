using System.Security.Claims;
using ElevateJobPortal.Services;
using ElevateJobPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevateJobPortal.Controllers;

[Authorize(Roles = "JobSeeker")]
public class ApplicationsController : Controller
{
    private readonly IApplicationService _applicationService;
    private readonly IJobService _jobService;

    public ApplicationsController(IApplicationService applicationService, IJobService jobService)
    {
        _applicationService = applicationService;
        _jobService = jobService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> Apply(int jobId)
    {
        var job = await _jobService.GetByIdAsync(jobId);
        if (job == null) return NotFound();
        return View(new ApplicationFormViewModel { JobId = job.Id, JobTitle = job.Title });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(ApplicationFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, message) = await _applicationService.ApplyAsync(model.JobId, UserId, model.CoverLetter);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(model);
        }

        TempData["Message"] = message;
        return RedirectToAction("Details", "Jobs", new { id = model.JobId });
    }
}