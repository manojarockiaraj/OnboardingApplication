using Microsoft.EntityFrameworkCore;
using OnboardingApp.Data;
using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public class OnBoardingRepository : IOnBoardingRepository
{
    private readonly AppDbContext _db;

    public OnBoardingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<OnBoardingList>> GetAllAsync()
    {
        return await _db.OnBoardingLists.AsNoTracking().ToListAsync();
    }

    public async Task<List<OnBoardingList>> GetActiveAsync()
    {
        return await _db.OnBoardingLists.AsNoTracking().Where(x => x.isactive).ToListAsync();
    }

    public async Task<OnBoardingList?> GetByIdAsync(int id)
    {
        return await _db.OnBoardingLists.FindAsync(id);
    }
}