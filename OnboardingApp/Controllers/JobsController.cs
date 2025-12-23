using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Models;

namespace OnboardingApp.Controllers;

public class JobsController : Controller
{
    // In-memory list to simulate data store
    private static readonly List<JobPosting> _jobs = new List<JobPosting>
    {
        new JobPosting { Id = 1, Title = "Software Engineer", Skills = "C#, ASP.NET Core, SQL", ClientName = "Acme Corp", ClientContact = "acme@example.com", Location = "London", Description = "Backend developer for API" },
        new JobPosting { Id = 2, Title = "Frontend Developer", Skills = "JavaScript, React, CSS", ClientName = "Beta LLC", ClientContact = "contact@beta.com", Location = "Coventry", Description = "Work on UI components" }
    };

    public IActionResult Index()
    {
        return View(_jobs);
    }

    public IActionResult Edit(int id)
    {
        var job = _jobs.FirstOrDefault(j => j.Id == id);
        if (job == null) return NotFound();
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, JobPosting updated)
    {
        if (id != updated.Id) return BadRequest();
        if (!ModelState.IsValid) return View(updated);

        var job = _jobs.FirstOrDefault(j => j.Id == id);
        if (job == null) return NotFound();

        // update fields
        job.Title = updated.Title;
        job.Skills = updated.Skills;
        job.ClientName = updated.ClientName;
        job.ClientContact = updated.ClientContact;
        job.Location = updated.Location;
        job.Description = updated.Description;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        return View(new JobPosting());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(JobPosting job)
    {
        if (!ModelState.IsValid) return View(job);
        job.Id = _jobs.Any() ? _jobs.Max(j => j.Id) + 1 : 1;
        _jobs.Add(job);
        return RedirectToAction(nameof(Index));
    }
}
