using ElevateJobPortal.Models;

namespace ElevateJobPortal.Repositories;

public interface ICompanyRepository
{
    Task AddAsync(Company company);
    Task<Company?> GetByUserIdAsync(string userId);
}