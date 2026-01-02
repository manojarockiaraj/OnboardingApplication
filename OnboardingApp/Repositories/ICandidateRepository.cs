using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public interface ICandidateRepository
{
    Task<List<Candidate>> GetAllAsync();
    Task<Candidate?> GetByIdAsync(int id);
    Task<int> AddAsync(Candidate candidate);
    Task UpdateAsync(Candidate candidate);
    Task AddOrUpdateCandidateStatusAsync(CandidateStatusEvaluation updated);
    Task<List<CandidateStatusEvaluation?>> GetCandidateEvaluation(int candidateId, int jobPostingId);
}
