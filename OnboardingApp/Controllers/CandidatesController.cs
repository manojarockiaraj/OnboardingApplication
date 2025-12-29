using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingApp.Data;
using OnboardingApp.Models;
using OnboardingApp.Repositories;
using OnboardingApp.Services;

namespace OnboardingApp.Controllers;

public class CandidatesController : Controller
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _db;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IJobRepository _jobRepository;

    public CandidatesController(IEmailService emailService, AppDbContext db, ICandidateRepository candidateRepository, IJobRepository jobRepository)
    {
        _emailService = emailService;
        _db = db;
    }
     
    public CandidatesController(IEmailService emailService, ICandidateRepository candidateRepository, IJobRepository jobRepository)
    {
        _emailService = emailService;

        _candidateRepository = candidateRepository;
        _jobRepository = jobRepository;
    }

    public async Task<IActionResult> Index(int? jobId)
    {
        var candidates = await _candidateRepository.GetAllAsync();
        ViewData["JobId"] = jobId;

        if (jobId.HasValue)
        {
            var job = await _jobRepository.GetByIdAsync(jobId.Value);
            if (job != null)
            {
                ViewData["JobTitle"] = job.Title;
                var jobSkills = job.Skills.Split(',').Select(s => s.Trim().ToLower()).Where(s => s.Length > 0).ToList();
                candidates = candidates.Where(c =>
                    c.Skills.Split(',').Select(s => s.Trim().ToLower()).Any(s => jobSkills.Contains(s))
                ).ToList();
            }
        }

        return View(candidates);
    }

     public async Task<IActionResult> Overview()
    {
        // Join candidate and candidatestatus (left join)
        //    var items = await _db.Candidates
        //.GroupJoin(
        //    _db.CandidateStatus,
        //    c => c.Id,
        //    s => s.ApplicantId,
        //    (c, statuses) => c
        //)
        //.Distinct()
        //.ToListAsync();


        var items = await _db.Candidates.ToListAsync();
        return View(items);
    }

    
    public async Task<IActionResult> Details(int id, int? jobId)
    {
 
        var candidate = await _candidateRepository.GetByIdAsync(id);
        if (candidate == null) return NotFound();

        ViewData["JobId"] = jobId;
        if (jobId.HasValue)
        {
            var job = await _jobRepository.GetByIdAsync(jobId.Value);
            if (job != null)
            {
                ViewData["JobClientEvaluation"] = job.ClientEvaluation;
                ViewData["JobTitle"] = job.Title;
            }
        }

        return View(candidate);
    }

    public async Task<IActionResult> EditStatus(int id, int? jobId)
    {
        var candidate = await _candidateRepository.GetByIdAsync(id);
        if (candidate == null) return NotFound();

        ViewData["JobId"] = jobId;
        if (jobId.HasValue)
        {
            var job = await _jobRepository.GetByIdAsync(jobId.Value);
            if (job != null)
            {
                ViewData["JobClientEvaluation"] = job.ClientEvaluation;
            }
        }

        return View(candidate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStatus(int id, Candidate updated, int? jobId)
    {
        if (id != updated.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            ViewData["JobId"] = jobId;
            if (jobId.HasValue)
            {
                var job = await _jobRepository.GetByIdAsync(jobId.Value);
                if (job != null)
                {
                    ViewData["JobClientEvaluation"] = job.ClientEvaluation;
                }
            }
            return View(updated);
        }

        // Ensure candidate exists in DB
        var candidateFromDb = await _candidateRepository.GetByIdAsync(id);
        if (candidateFromDb == null) return NotFound();

        // Update DB-backed candidate where possible
        candidateFromDb.Name = updated.Name;
        // only map fields that are persisted in DB table
        candidateFromDb.Skills = updated.Skills;
        candidateFromDb.Email = updated.Email;
        candidateFromDb.Phone = updated.Phone;
        candidateFromDb.Location = updated.Location;
        candidateFromDb.Summary = updated.Summary;
        //candidateFromDb.AvailableForInterview = updated.AvailableForInterview;

        await _candidateRepository.UpdateAsync(candidateFromDb);

        // Also update in-memory DataStore to keep existing app behavior for status fields
        var candidateInStore = DataStore.Candidates.FirstOrDefault(c => c.Id == id);
        if (candidateInStore != null)
        {
            candidateInStore.Name = updated.Name;
            candidateInStore.ResumeFilterStatus = updated.ResumeFilterStatus;
            candidateInStore.Level1Status = updated.Level1Status;
            candidateInStore.Level2Status = updated.Level2Status;
            candidateInStore.FinalStatus = updated.FinalStatus;
            candidateInStore.BpssStatus = updated.BpssStatus;

            candidateInStore.CTSInternalInterviewerName = updated.CTSInternalInterviewerName;
            candidateInStore.ClientInterviewerName = updated.ClientInterviewerName;
        }

        return RedirectToAction(nameof(Details), new { id = candidateFromDb.Id, jobId = jobId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitOnboarding(int id)
    {
        var candidate = await _candidateRepository.GetByIdAsync(id);
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
