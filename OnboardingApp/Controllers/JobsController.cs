using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Models;
using OnboardingApp.Data;
using OnboardingApp.Services;

namespace OnboardingApp.Controllers;

public class JobsController : Controller
{
    private readonly ISupabaseService _supabaseService;

    public JobsController(ISupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    private void PopulateDropdowns()
    {
        ViewBag.ClientContracts = new List<string> { "Multicapability", "ESFA" };
        ViewBag.WorkModels = new List<string> { "Hybrid", "Remote" };
    }

    public async Task<IActionResult> Index()
    {
        List<JobPosting> jobs;
        try
        {
            jobs = await _supabaseService.GetJobPostingsAsync() ?? new List<JobPosting>();
        }
        catch (Exception ex)
        {
            // Log the exception if you have logging; for now show a friendly error and return an empty list
            TempData["Error"] = "Unable to load job postings: " + ex.Message;
            jobs = new List<JobPosting>();
        }

        return View(jobs);
    }

    public IActionResult Edit(int id)
    {
        var job = DataStore.Jobs.FirstOrDefault(j => j.Id == id);
        if (job == null) return NotFound();
        PopulateDropdowns();
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, JobPosting updated)
    {
        if (id != updated.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(updated);
        }

        var job = DataStore.Jobs.FirstOrDefault(j => j.Id == id);
        if (job == null) return NotFound();

        // update fields
        job.Title = updated.Title;
        job.Skills = updated.Skills;
        job.ClientName = updated.ClientName;
        job.ClientContact = updated.ClientContact;
        job.Location = updated.Location;
        job.Description = updated.Description;
        job.StartDate = updated.StartDate;
        job.ClientEvaluation = updated.ClientEvaluation;
        job.ClientContract = updated.ClientContract;
        job.WorkModel = updated.WorkModel;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View(new JobPosting());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(JobPosting job)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(job);
        }
        job.Id = DataStore.Jobs.Any() ? DataStore.Jobs.Max(j => j.Id) + 1 : 1;
        DataStore.Jobs.Add(job);
        return RedirectToAction(nameof(Index));
    }
}
