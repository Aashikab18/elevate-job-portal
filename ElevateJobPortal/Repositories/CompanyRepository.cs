using ElevateJobPortal.Data;
using ElevateJobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ElevateJobPortal.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
    }

    public Task<Company?> GetByUserIdAsync(string userId)
    {
        return _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
    }
}