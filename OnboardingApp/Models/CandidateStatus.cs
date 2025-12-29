using System.ComponentModel.DataAnnotations;

namespace OnboardingApp.Models;

public class CandidateStatus
{
    public int StatusId { get; set; }
    
    public int ApplicantId { get; set; }

    public int JobPostingId { get; set; }

    public string? ResumeStatus { get; set; }

    public string? CtsInternalInterviewStatus { get; set; }

    public string? CtsInternalInterviewerName { get; set; }

    public string? ClientInterviewStatus { get; set; }

    public string? ClientInterviewerName { get; set; }

    public string? FinalStatus { get; set; }

    public string? SecurityCheckStatus { get; set; }

    public bool? OnboardingRequestSent { get; set; }

}
