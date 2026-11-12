using System.Security.Claims;
using ElevateJobPortal.Services;
using ElevateJobPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevateJobPortal.Controllers;

[Authorize(Roles = "Company")]
public class JobsController : Controller
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // ---------- Public pages (Feature 3) ----------
    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1)
    {
        var result = await _jobService.GetPagedAsync(page, 10);
        return View(result);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null) return NotFound();
        return View(job);
    }

    // ---------- Company pages (Feature 2) ----------
    public async Task<IActionResult> MyJobs()
    {
        var jobs = await _jobService.GetMyJobsAsync(UserId);
        return View(jobs);
    }

    [HttpGet]
    public IActionResult Create() => View(new JobFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var created = await _jobService.CreateAsync(model, UserId);
        if (!created)
        {
            ModelState.AddModelError("", "Company profile not found.");
            return View(model);
        }

        TempData["Message"] = "Job posted successfully.";
        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var job = await _jobService.GetOwnedJobAsync(id, UserId);
        if (job == null) return NotFound();

        return View(new JobFormViewModel
        {
            Title = job.Title,
            Description = job.Description,
            Location = job.Location,
            JobType = job.JobType,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            Deadline = job.Deadline
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, JobFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var updated = await _jobService.UpdateAsync(id, model, UserId);
        if (!updated) return NotFound();

        TempData["Message"] = "Job updated successfully.";
        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _jobService.GetOwnedJobAsync(id, UserId);
        if (job == null) return NotFound();
        return View(job);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _jobService.DeleteAsync(id, UserId);
        if (!deleted) return NotFound();

        TempData["Message"] = "Job deleted.";
        return RedirectToAction(nameof(MyJobs));
    }
}