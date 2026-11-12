using ElevateJobPortal.Models;
using ElevateJobPortal.Repositories;
using ElevateJobPortal.ViewModels;

namespace ElevateJobPortal.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly ICompanyRepository _companyRepository;

    public JobService(IJobRepository jobRepository, ICompanyRepository companyRepository)
    {
        _jobRepository = jobRepository;
        _companyRepository = companyRepository;
    }

    public async Task<bool> CreateAsync(JobFormViewModel model, string userId)
    {
        var company = await _companyRepository.GetByUserIdAsync(userId);
        if (company == null) return false;

        var job = new Job { CompanyId = company.Id };
        Apply(job, model);
        await _jobRepository.AddAsync(job);
        return true;
    }

    public async Task<List<Job>> GetMyJobsAsync(string userId)
    {
        var company = await _companyRepository.GetByUserIdAsync(userId);
        if (company == null) return new List<Job>();
        return await _jobRepository.GetByCompanyIdAsync(company.Id);
    }

    // Business rule: a company can only touch its own jobs
    public async Task<Job?> GetOwnedJobAsync(int id, string userId)
    {
        var company = await _companyRepository.GetByUserIdAsync(userId);
        var job = await _jobRepository.GetByIdAsync(id);
        if (company == null || job == null || job.CompanyId != company.Id) return null;
        return job;
    }

    public async Task<bool> UpdateAsync(int id, JobFormViewModel model, string userId)
    {
        var job = await GetOwnedJobAsync(id, userId);
        if (job == null) return false;

        Apply(job, model);
        job.UpdatedAt = DateTime.UtcNow;
        await _jobRepository.UpdateAsync(job);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var job = await GetOwnedJobAsync(id, userId);
        if (job == null) return false;

        await _jobRepository.DeleteAsync(job);
        return true;
    }

    // Paging: invalid page numbers are moved to the nearest valid page
    public async Task<PagedResult<Job>> GetPagedAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        var (items, total) = await _jobRepository.GetPagedAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        if (total > 0 && page > totalPages)
        {
            page = totalPages;
            (items, total) = await _jobRepository.GetPagedAsync(page, pageSize);
        }

        return new PagedResult<Job>
        {
            Items = items,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public Task<Job?> GetByIdAsync(int id)
    {
        return _jobRepository.GetByIdAsync(id);
    }

    private static void Apply(Job job, JobFormViewModel model)
    {
        job.Title = model.Title.Trim();
        job.Description = model.Description.Trim();
        job.Location = model.Location.Trim();
        job.JobType = model.JobType;
        job.SalaryMin = model.SalaryMin;
        job.SalaryMax = model.SalaryMax;
        job.Deadline = model.Deadline;
    }
}