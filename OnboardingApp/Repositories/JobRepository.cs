using Microsoft.EntityFrameworkCore;
using OnboardingApp.Data;
using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _db;

    public JobRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<JobPosting>> GetAllAsync()
    {
        return await _db.JobPostings.AsNoTracking().ToListAsync();
    }

    public async Task<JobPosting?> GetByIdAsync(int id)
    {
        return await _db.JobPostings.FindAsync(id);
    }

    public async Task AddAsync(JobPosting job)
    {
        if (job.StartDate.HasValue)
        {
            job.StartDate = DateTime.SpecifyKind(job.StartDate.Value, DateTimeKind.Utc);
        }
        _db.JobPostings.Add(job);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobPosting job)
    {
        if (job.StartDate.HasValue)
        {
            job.StartDate = DateTime.SpecifyKind(job.StartDate.Value, DateTimeKind.Utc);
        }
        _db.JobPostings.Update(job);
        await _db.SaveChangesAsync();
    }
}
