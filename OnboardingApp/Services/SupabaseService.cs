using Supabase;
using OnboardingApp.Models;
using OnboardingApp.Data;

namespace OnboardingApp.Services;

public interface ISupabaseService
{
    Task<List<JobPosting>> GetJobPostingsAsync();
}

public class SupabaseService : ISupabaseService
{
    private readonly Client _client;

    public SupabaseService(Client client)
    {
        _client = client;
    }

    public async Task<List<JobPosting>> GetJobPostingsAsync()
    {
        try
        {
            // Query supabase table 'jobpostings' (ensure table name matches)
            var resp = await _client.From<JobPosting>().Get();
            var models = resp.Models?.ToList() ?? new List<JobPosting>();
            if (models.Count == 0)
            {
                return DataStore.Jobs; // fallback
            }
            return models;
        }
        catch
        {
            return DataStore.Jobs;
        }
    }
}

public class NullSupabaseService : ISupabaseService
{
    public Task<List<JobPosting>> GetJobPostingsAsync()
    {
        return Task.FromResult(new List<JobPosting>());
    }
}
