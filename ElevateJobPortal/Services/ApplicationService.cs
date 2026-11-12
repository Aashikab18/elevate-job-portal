using ElevateJobPortal.Models;
using ElevateJobPortal.Repositories;

namespace ElevateJobPortal.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ICompanyRepository _companyRepository;

    public ApplicationService(
        IApplicationRepository applicationRepository,
        IJobRepository jobRepository,
        ICompanyRepository companyRepository)
    {
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
        _companyRepository = companyRepository;
    }

    public async Task<(bool Success, string Message)> ApplyAsync(int jobId, string userId, string coverLetter)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null) return (false, "Job not found.");

        // Business rule: only open jobs can be applied to
        if (!job.IsActive || job.IsExpired()) return (false, "This job is closed.");

        // Business rule: one application per job seeker per job
        if (await _applicationRepository.ExistsAsync(jobId, userId))
            return (false, "You have already applied for this job.");

        await _applicationRepository.AddAsync(new JobApplication
        {
            JobId = jobId,
            ApplicantId = userId,
            CoverLetter = coverLetter.Trim()
        });
        return (true, "Your application was submitted.");
    }

    public async Task<List<JobApplication>> GetForCompanyAsync(string userId)
    {
        var company = await _companyRepository.GetByUserIdAsync(userId);
        if (company == null) return new List<JobApplication>();
        return await _applicationRepository.GetForCompanyAsync(company.Id);
    }
}