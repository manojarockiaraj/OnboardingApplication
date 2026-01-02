using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Data;
using OnboardingApp.Models;
using OnboardingApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OnboardingApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOnBoardingRepository _onboardRepo;
    private readonly IJobRepository _jobRepo;
    private readonly AppDbContext _db;

    public HomeController(ILogger<HomeController> logger, IOnBoardingRepository onboardRepo, IJobRepository jobRepo, AppDbContext db)
    {
        _logger = logger;
        _onboardRepo = onboardRepo;
        _jobRepo = jobRepo;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        // Get onboarding items
        var onboardingItems = await _onboardRepo.GetAllAsync();
        ViewData["OnBoardingItems"] = onboardingItems;

        // Get job postings count
        var jobPostings = await _jobRepo.GetAllAsync();
        ViewData["OpenPositions"] = jobPostings.Count;

        // Get all candidates count
        var totalCandidates = await _db.Candidates.CountAsync();
        ViewData["TotalCandidates"] = totalCandidates;

        // Get active applications (candidates with at least one Passed status or Pending status)
        var activeApplications = await _db.CandidateStatusEvaluation
            //.Where(c => c.ResumeFilterStatus == StageStatus.Pending || c.ResumeFilterStatus == StageStatus.Passed ||
                        //c.Level1Status == StageStatus.Pending || c.Level1Status == StageStatus.Passed ||
                        //c.Level2Status == StageStatus.Pending || c.Level2Status == StageStatus.Passed)
            .CountAsync();
        ViewData["ActiveApplications"] = activeApplications;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> OnBoarding()
    {

        var onboardingItems = await _onboardRepo.GetAllAsync();
        ViewData["OnBoardingItems"] = onboardingItems;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
