using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Models;
using OnboardingApp.Data;
using Microsoft.EntityFrameworkCore;
using OnboardingApp.Repositories;

namespace OnboardingApp.Controllers;

public class JobsController : Controller
{
    private readonly IJobRepository _repo;

    public JobsController(IJobRepository repo)
    {
        _repo = repo;
    }

    private void PopulateDropdowns()
    {
        ViewBag.ClientContracts = new List<string> { "Multicapability", "ESFA" };
        ViewBag.WorkModels = new List<string> { "Hybrid", "Remote" };
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await _repo.GetAllAsync();
        return View(jobs);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var job = await _repo.GetByIdAsync(id);
        if (job == null) return NotFound();
        PopulateDropdowns();
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, JobPosting updated)
    {
        if (id != updated.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(updated);
        }

        var job = await _repo.GetByIdAsync(id);
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
        job.RequirementFulfilled = updated.RequirementFulfilled;

        await _repo.UpdateAsync(job);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View(new JobPosting());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobPosting job)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(job);
        }
        await _repo.AddAsync(job);
        return RedirectToAction(nameof(Index));
    }
}
