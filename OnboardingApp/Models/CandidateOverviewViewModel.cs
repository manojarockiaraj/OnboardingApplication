namespace OnboardingApp.Models;

public class CandidateOverviewViewModel
{
    public int Id { get; set; } // candidate ID
    public int StatusId { get; set; }
    public int JobPostingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AppliedFor { get; set; } = string.Empty; // using Skills for demo

    // Status fields from CandidateStatus
    public string? ResumeStatus { get; set; }
    public string? CtsInternalInterviewStatus { get; set; }
    public string? CtsInternalInterviewerName { get; set; }
    public string? ClientInterviewStatus { get; set; }
    public string? ClientInterviewerName { get; set; }
    public string? FinalStatus { get; set; }
    public string? SecurityCheckStatus { get; set; }
    public bool? OnboardingRequestSent { get; set; }
}