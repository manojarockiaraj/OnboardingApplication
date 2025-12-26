using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public interface IJobRepository
{
    Task<List<JobPosting>> GetAllAsync();
    Task<JobPosting?> GetByIdAsync(int id);
    Task AddAsync(JobPosting job);
    Task UpdateAsync(JobPosting job);
}
