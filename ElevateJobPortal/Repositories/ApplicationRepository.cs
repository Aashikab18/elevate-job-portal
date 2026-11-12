using ElevateJobPortal.Data;
using ElevateJobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ElevateJobPortal.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(int jobId, string applicantId)
    {
        return _context.JobApplications.AnyAsync(a => a.JobId == jobId && a.ApplicantId == applicantId);
    }

    public Task<List<JobApplication>> GetForCompanyAsync(int companyId)
    {
        return _context.JobApplications
            .Include(a => a.Job)
            .Include(a => a.Applicant)
            .Where(a => a.Job!.CompanyId == companyId)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();
    }
}