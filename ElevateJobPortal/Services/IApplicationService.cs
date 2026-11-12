using ElevateJobPortal.Models;

namespace ElevateJobPortal.Services;

public interface IApplicationService
{
    Task<(bool Success, string Message)> ApplyAsync(int jobId, string userId, string coverLetter);
    Task<List<JobApplication>> GetForCompanyAsync(string userId);
}