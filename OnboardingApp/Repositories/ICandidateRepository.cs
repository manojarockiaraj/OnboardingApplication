using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public interface ICandidateRepository
{
    Task<List<Candidate>> GetAllAsync();
    Task<Candidate?> GetByIdAsync(int id);
    Task UpdateAsync(Candidate candidate);
    Task AddOrUpdateCandidateStatusAsync(CandidateStatusEvaluation updated);
    Task<List<CandidateStatusEvaluation?>> GetCandidateEvaluation(int candidateId, int jobPostingId);
}
