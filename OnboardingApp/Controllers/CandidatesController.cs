using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Data;
using OnboardingApp.Models;
using OnboardingApp.Services;

namespace OnboardingApp.Controllers;

public class CandidatesController : Controller
{
    private readonly IEmailService _emailService;

    public CandidatesController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public IActionResult Index(int? jobId)
    {
        var candidates = DataStore.Candidates.AsEnumerable();
        ViewData["JobId"] = jobId;

        if (jobId.HasValue)
        {
            var job = DataStore.Jobs.FirstOrDefault(j => j.Id == jobId.Value);
            if (job != null)
            {
                ViewData["JobTitle"] = job.Title;
                // match candidates by overlapping skills (simple comma-separated match)
                var jobSkills = job.Skills.Split(',').Select(s => s.Trim().ToLower()).Where(s => s.Length > 0).ToList();
                candidates = candidates.Where(c =>
                    c.Skills.Split(',').Select(s => s.Trim().ToLower()).Any(s => jobSkills.Contains(s))
                );
            }
        }

        return View(candidates);
    }

    public IActionResult Details(int id, int? jobId)
    {
        var candidate = DataStore.Candidates.FirstOrDefault(c => c.Id == id);
        if (candidate == null) return NotFound();

        ViewData["JobId"] = jobId;
        if (jobId.HasValue)
        {
            var job = DataStore.Jobs.FirstOrDefault(j => j.Id == jobId.Value);
            if (job != null)
            {
                ViewData["JobClientEvaluation"] = job.ClientEvaluation;
                ViewData["JobTitle"] = job.Title;
            }
        }

        return View(candidate);
    }

    public IActionResult EditStatus(int id, int? jobId)
    {
        var candidate = DataStore.Candidates.FirstOrDefault(c => c.Id == id);
        if (candidate == null) return NotFound();

        ViewData["JobId"] = jobId;
        if (jobId.HasValue)
        {
            var job = DataStore.Jobs.FirstOrDefault(j => j.Id == jobId.Value);
            if (job != null)
            {
                ViewData["JobClientEvaluation"] = job.ClientEvaluation;
            }
        }

        return View(candidate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditStatus(int id, Candidate updated, int? jobId)
    {
        if (id != updated.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            ViewData["JobId"] = jobId;
            if (jobId.HasValue)
            {
                var job = DataStore.Jobs.FirstOrDefault(j => j.Id == jobId.Value);
                if (job != null)
                {
                    ViewData["JobClientEvaluation"] = job.ClientEvaluation;
                }
            }
            return View(updated);
        }

        var candidate = DataStore.Candidates.FirstOrDefault(c => c.Id == id);
        if (candidate == null) return NotFound();

        candidate.Name = updated.Name;
        candidate.ResumeFilterStatus = updated.ResumeFilterStatus;
        candidate.Level1Status = updated.Level1Status;
        candidate.Level2Status = updated.Level2Status;
        candidate.FinalStatus = updated.FinalStatus;
        candidate.BpssStatus = updated.BpssStatus;

        // Save interviewer names
        candidate.CTSInternalInterviewerName = updated.CTSInternalInterviewerName;
        candidate.ClientInterviewerName = updated.ClientInterviewerName;

        return RedirectToAction(nameof(Details), new { id = candidate.Id, jobId = jobId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitOnboarding(int id)
    {
        var candidate = DataStore.Candidates.FirstOrDefault(c => c.Id == id);
        if (candidate == null) return NotFound();

        // Only allow if FinalStatus and BpssStatus are Passed
        if (candidate.FinalStatus != StageStatus.Passed || candidate.BpssStatus != StageStatus.Passed)
        {
            TempData["Error"] = "Candidate must have Final Status and BPSS Status as Passed to submit onboarding request.";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        try
        {
            await _emailService.SendOnboardingRequestAsync(candidate);
            TempData["Success"] = "Onboarding request submitted via email.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Failed to send onboarding email: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id = id });
    }
}
