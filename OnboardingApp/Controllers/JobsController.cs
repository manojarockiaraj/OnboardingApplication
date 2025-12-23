using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Models;
using OnboardingApp.Data;

namespace OnboardingApp.Controllers;

public class JobsController : Controller
{
    private void PopulateDropdowns()
    {
        ViewBag.ClientContracts = new List<string> { "Multicapability", "ESFA" };
        ViewBag.WorkModels = new List<string> { "Hybrid", "Remote" };
    }

    public IActionResult Index()
    {
        return View(DataStore.Jobs);
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
