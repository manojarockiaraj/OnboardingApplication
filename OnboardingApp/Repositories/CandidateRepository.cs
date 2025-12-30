using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingApp.Data;
using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly AppDbContext _db;

    public CandidateRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Candidate>> GetAllAsync()
    {
        return await _db.Candidates.AsNoTracking().ToListAsync();
    }

    public async Task<Candidate?> GetByIdAsync(int id)
    {
        return await _db.Candidates.FindAsync(id);
    }

    public async Task<List<CandidateStatusEvaluation?>> GetCandidateEvaluation(int candidateId, int jobPostingId)
    {
        return await _db.CandidateStatusEvaluation
                     .Where(c => c.CandidateId == candidateId &&
                                 c.JobPostingId == jobPostingId)
                     .AsNoTracking()
                     .ToListAsync();
    }

    public async Task UpdateAsync(Candidate candidate)
    {
        _db.Candidates.Update(candidate);
        await _db.SaveChangesAsync();
    }
 
    public async Task AddOrUpdateCandidateStatusAsync(CandidateStatusEvaluation updated)
    {
        // Try to get existing evaluation for the candidate & job
        var evaluationInDb = await _db.CandidateStatusEvaluation
            .FirstOrDefaultAsync(e => e.CandidateId == updated.CandidateId
                                   && e.JobPostingId == updated.JobPostingId);

        if (evaluationInDb == null)
        {
            // Create new record if not exists
            evaluationInDb = new CandidateStatusEvaluation
            {
                CandidateId = updated.CandidateId,
                JobPostingId = updated.JobPostingId,
                CreatedDate = DateTime.UtcNow
            };
            _db.CandidateStatusEvaluation.Add(evaluationInDb);
        }

        // Update fields
        evaluationInDb.ResumeStatus = updated.ResumeStatus;
        evaluationInDb.ResumeReviewedBy = updated.ResumeReviewedBy;
        evaluationInDb.ResumeReviewedDate = updated.ResumeReviewedDate;

        evaluationInDb.CtsInternalInterviewStatus = updated.CtsInternalInterviewStatus;
        evaluationInDb.CtsInternalInterviewerName = updated.CtsInternalInterviewerName;
        evaluationInDb.CtsInternalInterviewDate = updated.CtsInternalInterviewDate;
        evaluationInDb.CtsInternalFeedback = updated.CtsInternalFeedback;

        evaluationInDb.ClientInterviewStatus = updated.ClientInterviewStatus;
        evaluationInDb.ClientInterviewerName = updated.ClientInterviewerName;
        evaluationInDb.ClientInterviewDate = updated.ClientInterviewDate;
        evaluationInDb.ClientFeedback = updated.ClientFeedback;

        evaluationInDb.FinalStatus = updated.FinalStatus;
        evaluationInDb.FinalDecisionDate = updated.FinalDecisionDate;

        evaluationInDb.SecurityCheckStatus = updated.SecurityCheckStatus;
        evaluationInDb.SecurityCheckCompletedDate = updated.SecurityCheckCompletedDate;

        evaluationInDb.OnboardingRequestSent = updated.OnboardingRequestSent;
        evaluationInDb.OnboardingRequestDate = updated.OnboardingRequestDate;

        evaluationInDb.UpdatedDate = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
    }


}
