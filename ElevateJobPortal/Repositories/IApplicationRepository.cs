using ElevateJobPortal.Models;

namespace ElevateJobPortal.Repositories;

public interface IApplicationRepository
{
    Task AddAsync(JobApplication application);
    Task<bool> ExistsAsync(int jobId, string applicantId);
    Task<List<JobApplication>> GetForCompanyAsync(int companyId);
}