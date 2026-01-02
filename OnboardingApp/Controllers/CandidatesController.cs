using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingApp.Data;
using OnboardingApp.Models;
using OnboardingApp.Repositories;
using OnboardingApp.Services;
using Microsoft.AspNetCore.Authorization;

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
        _candidateRepository = candidateRepository;
        _jobRepository = jobRepository;
    }

    public async Task<IActionResult> Index(int? jobId)
    {
        var candidates = await _candidateRepository.GetAllAsync();
        ViewData["JobId"] = jobId;
        return View(candidates);
    }

    [Authorize(Roles = "AccountManager")]
    public async Task<IActionResult> AssignCandidates(int jobId)
    {
        var candidates = await _candidateRepository.GetAllAsync();
        ViewData["JobId"] = jobId;

        if (jobId!=0)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job != null)
            {
                ViewData["JobTitle"] = string.Concat(job.Title, " ", job.Skills);
                var jobSkills = job.Skills.Split(',').Select(s => s.Trim().ToLower()).Where(s => s.Length > 0).ToList();
                candidates = candidates.Where(c =>
                    c.Skills.Split(',').Select(s => s.Trim().ToLower()).Any(s => jobSkills.Contains(s))
                ).ToList();

                if (candidates.Any())
                    candidates[0].JobPostingDetails = job;
            }
        }

        return View(candidates);
    }

    [Authorize(Roles = "AccountManager")]
    public async Task<IActionResult> MapEmployeeToJob(int id, int? jobId)
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

                candidate.JobPostingDetails = job;
            }
        }

        if (jobId.HasValue)
            candidate.CandidateStatusEvaluations = await _candidateRepository.GetCandidateEvaluation(id, jobId.Value);

        return View(candidate);
    }

    public async Task<IActionResult> Overview()
    {
        // Get candidates with their latest evaluation
        var items = await (
                 from c in _db.Candidates
                 join e in _db.CandidateStatusEvaluation
                     on c.Id equals e.CandidateId
                 join j in _db.JobPostings
                     on e.JobPostingId equals j.Id
                 select new
                 {
                     Candidate = c,
                     Evaluation = e,
                     JobPosting = j
                 }
             ).ToListAsync();



        // Map to CandidateOverviewViewModel for the view
        var model = items.Select(x => new CandidateOverviewViewModel
        {
            Id = x.Candidate.Id,
            StatusId = x.Evaluation.EvaluationId,
            JobPostingId = x.Evaluation.JobPostingId,
            Name = x.Candidate.Name,
            Email = x.Candidate.Email,
            AppliedFor = x.JobPosting.Skills, // or JobTitle, whichever you prefer
            

            // Status fields
            ResumeStatus = x.Evaluation?.ResumeStatus.ToString() ?? "Submitted",
            CtsInternalInterviewStatus = x.Evaluation?.CtsInternalInterviewStatus.ToString() ?? "NA",
            CtsInternalInterviewerName = x.Evaluation?.CtsInternalInterviewerName,
            ClientInterviewStatus = x.Evaluation?.ClientInterviewStatus.ToString() ?? "NA",
            ClientInterviewerName = x.Evaluation?.ClientInterviewerName,
            FinalStatus = x.Evaluation?.FinalStatus.ToString() ?? "NA",
            SecurityCheckStatus = x.Evaluation?.SecurityCheckStatus.ToString() ?? "NA",
            OnboardingRequestSent = x.Evaluation?.OnboardingRequestSent ?? false
        }).ToList().OrderBy(a=>a.Email);

        return View(model);
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

        if (jobId.HasValue)
            candidate.CandidateStatusEvaluations = await _candidateRepository.GetCandidateEvaluation(id, jobId.Value);

        return View(candidate);
    }

    [Authorize(Roles = "AccountManager")]
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

        if (jobId.HasValue)
            candidate.CandidateStatusEvaluations = await _candidateRepository.GetCandidateEvaluation(id, jobId.Value);

        return View(candidate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "AccountManager")]
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
                //return View(updated);
            }

            // Ensure candidate exists in DB
            var candidateFromDb = await _candidateRepository.GetByIdAsync(id);
            if (candidateFromDb == null) return NotFound();

            var evaluation = new CandidateStatusEvaluation
            {
                CandidateId = candidateFromDb.Id,
                JobPostingId = jobId.Value,
                ResumeStatus = updated.ResumeFilterStatus,
                CtsInternalInterviewStatus = updated.Level1Status,
                ClientInterviewStatus = updated.Level2Status,
                FinalStatus = updated.FinalStatus,
                SecurityCheckStatus = updated.BpssStatus,
                CtsInternalInterviewerName = updated.CTSInternalInterviewerName,
                ClientInterviewerName = updated.ClientInterviewerName
            };

            await _candidateRepository. AddOrUpdateCandidateStatusAsync(evaluation);


            return RedirectToAction(nameof(Details), new { id = candidateFromDb.Id, jobId = jobId });
        }

    [HttpPost]
    [Authorize(Roles = "AccountManager")]
    public async Task<IActionResult> UpdateResumeStatus(int JobPostingId, List<Candidate> Candidates)
    {
        List<string> _error = new List<string>();

        // Get selected candidates
        var selectedCandidates = Candidates
            .Where(c => c.IsSelected)   
            .ToList();

        if (!selectedCandidates.Any())
        {
            TempData["Error"] = "Please select at least one candidate.";
            return RedirectToAction("Index", new { jobId = JobPostingId });
        }

        foreach (var selected in selectedCandidates)
        {
            // Get candidate from DB (assuming you have _candidateRepository)
            var candidateFromDb = await _candidateRepository.GetByIdAsync(selected.Id);
            if (candidateFromDb == null) continue;

            // Create or update CandidateStatusEvaluation
            var evaluation = new CandidateStatusEvaluation
            {
                CandidateId = candidateFromDb.Id,
                JobPostingId = JobPostingId,
                ResumeStatus = ResumeStatus.UnderReview, // or from your logic
                UpdatedDate = DateTime.UtcNow
            };

            // Repository handles Add or Update
            await _candidateRepository.AddOrUpdateCandidateStatusAsync(evaluation);
        }

        TempData["Success"] = "Resume status updated successfully.";
        return RedirectToAction("Index", new { jobId = JobPostingId });
    }

    [Authorize(Roles = "AccountManager")]
    public async Task<IActionResult> AllocateCandidate(int jobPostingId, int candidateId)
    {
        var candidateFromDb = await _candidateRepository.GetByIdAsync(candidateId);

        if (candidateFromDb == null)
            return RedirectToAction("Index", new { jobId = jobPostingId });

        var evaluation = new CandidateStatusEvaluation
        {
            CandidateId = candidateFromDb.Id,
            JobPostingId = jobPostingId,
            ResumeStatus = ResumeStatus.UnderReview, // or from your logic
            UpdatedDate = DateTime.UtcNow
        };

        // Repository handles Add or Update
        await _candidateRepository.AddOrUpdateCandidateStatusAsync(evaluation);

       

        TempData["Success"] = "Allocation status updated successfully.";
        ViewData["JobId"] = jobPostingId; // pass extra info via ViewData
        return View("MapEmployeeToJob", candidateFromDb);

    }

    [Authorize(Roles = "Operation,AccountManager")]
    public IActionResult Create()
    {
        PopulateGradeDropdown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Operation,AccountManager")]
    public async Task<IActionResult> Create(Candidate candidate)
    {
        if (!ModelState.IsValid)
        {
            PopulateGradeDropdown();
            return View(candidate);
        }

        await _candidateRepository.AddAsync(candidate);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Operation,AccountManager")]
    public async Task<IActionResult> Edit(int id)
    {
        var candidate = await _candidateRepository.GetByIdAsync(id);
        if (candidate == null)
            return NotFound();

        PopulateGradeDropdown();
        return View(candidate);
    }

    // POST: Candidates/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Operation,AccountManager")]
    public async Task<IActionResult> Edit(int id, Candidate candidate)
    {
        if (id != candidate.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            PopulateGradeDropdown();
            return View(candidate);
        }

        await _candidateRepository.UpdateAsync(candidate);
        return RedirectToAction(nameof(Index));
    }

    private void PopulateGradeDropdown()
    {
        ViewBag.Grades = EmployeeGrades.All;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "AccountManager")]
    public async Task<IActionResult> SubmitOnboarding(int id)
    {
        var candidate = await _candidateRepository.GetByIdAsync(id);
        if (candidate == null) return NotFound();

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
