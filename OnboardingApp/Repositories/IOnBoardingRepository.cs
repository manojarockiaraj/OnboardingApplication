using OnboardingApp.Models;

namespace OnboardingApp.Repositories;

public interface IOnBoardingRepository
{
    Task<List<OnBoardingList>> GetAllAsync();
    Task<List<OnBoardingList>> GetActiveAsync();
    Task<OnBoardingList?> GetByIdAsync(int id);
}