using ElevateJobPortal.Models;

namespace ElevateJobPortal.Repositories;

public interface IJobRepository
{
    Task AddAsync(Job job);
    Task<Job?> GetByIdAsync(int id);
    Task<List<Job>> GetByCompanyIdAsync(int companyId);
    Task<(List<Job> Items, int Total)> GetPagedAsync(int page, int pageSize);
    Task UpdateAsync(Job job);
    Task DeleteAsync(Job job);
}