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

                if(candidates.Any())
                    candidates[0].JobPostingDetails = job;
            }
        }

        //var candidates = await _candidateRepository.GetAllAsync();
        //ViewData["JobId"] = jobId;

        //if (jobId.HasValue)
        //{
        //    var job = await _jobRepository.GetByIdAsync(jobId.Value);
        //    if (job != null)
        //    {
        //        ViewData["JobTitle"] = job.Title;

        //        // Prepare job skills for comparison
        //        var jobSkills = job.Skills
        //            .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //            .Select(s => s.Trim().ToLower())
        //            .ToList();

        //        // Filter candidates based on matching skills
        //        candidates = candidates
        //            .Where(c => c.Skills
        //                .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //                .Select(s => s.Trim().ToLower())
        //                .Any(s => jobSkills.Contains(s)))
        //            .ToList();

        //        // Update each candidate with their evaluation for this job
        //        foreach (var candidate in candidates)
        //        {
        //            candidate.CandidateStatusEvaluations = await _candidateRepository.GetCandidateEvaluation(candidate.Id, jobId.Value);
        //        }
        //    }
        //}


        return View(candidates);
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
        }).ToList();

        return View(model);
    }


    public async Task<IActionResult> Overview1()
    {
        // Get candidates with their latest evaluation
        var items = await (
            from c in _db.Candidates
            join e in _db.CandidateStatusEvaluation
                on c.Id equals e.CandidateId into evals
            from latestEval in evals
                .OrderByDescending(x => x.UpdatedDate ?? x.CreatedDate)
                .Take(1)
                .DefaultIfEmpty()
            select new
            {
                Candidate = c,
                Evaluation = latestEval
            }
        ).ToListAsync();

        // Map to full model for the view
        var model = items.Select(x => new CandidateStatusEvaluation
        {
            EvaluationId = x.Evaluation?.EvaluationId ?? 0,
            CandidateId = x.Candidate.Id,
            JobPostingId = x.Evaluation?.JobPostingId ?? 0,
            ResumeStatus = x.Evaluation.ResumeStatus,
            ResumeReviewedBy = x.Evaluation?.ResumeReviewedBy,
            ResumeReviewedDate = x.Evaluation?.ResumeReviewedDate,
            CtsInternalInterviewStatus = x.Evaluation.CtsInternalInterviewStatus,
            CtsInternalInterviewerName = x.Evaluation?.CtsInternalInterviewerName,
            CtsInternalInterviewDate = x.Evaluation?.CtsInternalInterviewDate,
            CtsInternalFeedback = x.Evaluation?.CtsInternalFeedback,
            ClientInterviewStatus = x.Evaluation.ClientInterviewStatus,
            ClientInterviewerName = x.Evaluation?.ClientInterviewerName,
            ClientInterviewDate = x.Evaluation?.ClientInterviewDate,
            ClientFeedback = x.Evaluation?.ClientFeedback,
            FinalStatus = x.Evaluation.FinalStatus,
            FinalDecisionDate = x.Evaluation?.FinalDecisionDate,
            SecurityCheckStatus = x.Evaluation.SecurityCheckStatus,
            SecurityCheckCompletedDate = x.Evaluation?.SecurityCheckCompletedDate,
            OnboardingRequestSent = x.Evaluation?.OnboardingRequestSent ?? false,
            OnboardingRequestDate = x.Evaluation?.OnboardingRequestDate,
            CreatedDate = x.Evaluation?.CreatedDate,
            UpdatedDate = x.Evaluation?.UpdatedDate
        }).ToList();

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
            //candidateFromDb.Name = updated.Name;
            //// only map fields that are persisted in DB table
            //candidateFromDb.Skills = updated.Skills;
            //candidateFromDb.Email = updated.Email;
            //candidateFromDb.Phone = updated.Phone;
            //candidateFromDb.Location = updated.Location;
            //candidateFromDb.Summary = updated.Summary;
            //candidateFromDb.AvailableForInterview = updated.AvailableForInterview;

            //await _candidateRepository.UpdateAsync(candidateFromDb);

        // Also update in-memory DataStore to keep existing app behavior for status fields
        //var candidateInStore = DataStore.Candidates.FirstOrDefault(c => c.Id == id);
        //if (candidateInStore != null)
        //{
        //    candidateInStore.Name = updated.Name;
        //    candidateInStore.ResumeFilterStatus = updated.ResumeFilterStatus;
        //    candidateInStore.Level1Status = updated.Level1Status;
        //    candidateInStore.Level2Status = updated.Level2Status;
        //    candidateInStore.FinalStatus = updated.FinalStatus;
        //    candidateInStore.BpssStatus = updated.BpssStatus;

        //    candidateInStore.CTSInternalInterviewerName = updated.CTSInternalInterviewerName;
        //    candidateInStore.ClientInterviewerName = updated.ClientInterviewerName;
        //}

        //await _candidateRepository.UpdateAsync(candidateFromDb);

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
    public async Task<IActionResult> UpdateResumeStatus(int JobPostingId, List<Candidate> Candidates)
    {
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



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitOnboarding(int id)
    {
        var candidate = await _candidateRepository.GetByIdAsync(id);
        if (candidate == null) return NotFound();

        // Only allow if FinalStatus and BpssStatus are Passed
        //if (candidate.FinalStatus != StageStatus.Passed || candidate.BpssStatus != StageStatus.Passed)
        //{
        //    TempData["Error"] = "Candidate must have Final Status and BPSS Status as Passed to submit onboarding request.";
        //    return RedirectToAction(nameof(Details), new { id = id });
        //}

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
