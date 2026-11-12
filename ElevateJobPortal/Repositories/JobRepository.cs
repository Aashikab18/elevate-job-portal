using ElevateJobPortal.Data;
using ElevateJobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ElevateJobPortal.Repositories;

public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
    }

    public Task<Job?> GetByIdAsync(int id)
    {
        return _context.Jobs.Include(j => j.Company).FirstOrDefaultAsync(j => j.Id == id);
    }

    public Task<List<Job>> GetByCompanyIdAsync(int companyId)
    {
        return _context.Jobs
            .Where(j => j.CompanyId == companyId)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    // Only active jobs whose deadline has not passed, newest first
    public async Task<(List<Job> Items, int Total)> GetPagedAsync(int page, int pageSize)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var query = _context.Jobs
            .Include(j => j.Company)
            .Where(j => j.IsActive && j.Deadline >= today)
                        .OrderByDescending(j => j.CreatedAt)
            .ThenByDescending(j => j.Id);

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Job job)
    {
        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
    }
}