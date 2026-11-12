using ElevateJobPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElevateJobPortal.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Company>().HasIndex(c => c.UserId).IsUnique();
        builder.Entity<Company>()
            .HasOne(c => c.User).WithOne()
            .HasForeignKey<Company>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Job>(e =>
        {
            e.Property(j => j.Title).HasMaxLength(150).IsRequired();
            e.Property(j => j.Location).HasMaxLength(100).IsRequired();
            e.Property(j => j.JobType).HasConversion<string>().HasMaxLength(20);
            e.Property(j => j.SalaryMin).HasPrecision(12, 2);
            e.Property(j => j.SalaryMax).HasPrecision(12, 2);
            e.HasOne(j => j.Company).WithMany()
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(j => new { j.IsActive, j.CreatedAt });
        });

        builder.Entity<JobApplication>(e =>
        {
            e.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(a => a.Job).WithMany()
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(a => a.Applicant).WithMany()
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(a => new { a.JobId, a.ApplicantId }).IsUnique();
        });
    }
}