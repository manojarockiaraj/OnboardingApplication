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

    public async Task UpdateAsync(Candidate candidate)
    {
        _db.Candidates.Update(candidate);
        await _db.SaveChangesAsync();
    }
}
