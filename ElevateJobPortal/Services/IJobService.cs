using ElevateJobPortal.Models;
using ElevateJobPortal.ViewModels;

namespace ElevateJobPortal.Services;

public interface IJobService
{
    Task<bool> CreateAsync(JobFormViewModel model, string userId);
    Task<List<Job>> GetMyJobsAsync(string userId);
    Task<Job?> GetOwnedJobAsync(int id, string userId);
    Task<bool> UpdateAsync(int id, JobFormViewModel model, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<PagedResult<Job>> GetPagedAsync(int page, int pageSize);
    Task<Job?> GetByIdAsync(int id);
}